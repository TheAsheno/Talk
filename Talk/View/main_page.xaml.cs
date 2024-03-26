using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Talk.ViewModel;

namespace Talk.View
{
    /// <summary>
    /// main_page.xaml 的交互逻辑
    /// </summary>
    public partial class main_page : Page
    {
        DispatcherTimer timer;
        MainViewModel mainViewModel;
        public main_page(string userid)
        {
            InitializeComponent();
            mainViewModel = new MainViewModel(userid);
            DataContext = mainViewModel;
            mainViewModel.loadHeadInfo();
            mainViewModel.loadContent();
            displayContent();
            textBlockout.Text = mainViewModel.homeModel.HeadInfo[Currentnum].Text;
            textBlockout.Tag = mainViewModel.homeModel.HeadInfo[Currentnum].Hid;
            textBlockoutauthor.Text = "——" + mainViewModel.homeModel.HeadInfo[Currentnum].Author;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private home _parentWin;
        public home ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }
        private int _Currentnum = 0;
        public int Currentnum
        {
            get { return _Currentnum; }
            set
            {
                _Currentnum = value;
            }
        }
        private int _Nextnum = 0;
        public int Nextnum
        {
            get { return _Nextnum; }
            set
            {
                _Nextnum = value;
            }
        }
        private void displayContent()
        {
            for (int i = 0; i < mainViewModel.homeModel.NewContent.Count(); i++)
            {
                DockPanel dockPanel = new DockPanel();
                dockPanel.LastChildFill = false;
                TextBlock textBlock1 = new TextBlock();
                textBlock1.Text = mainViewModel.homeModel.NewContent[i].Title;
                textBlock1.Tag = mainViewModel.homeModel.NewContent[i].Pid;
                textBlock1.Style = (Style)FindResource("contentblock");
                DockPanel.SetDock(textBlock1, Dock.Left);
                dockPanel.Children.Add(textBlock1);

                TextBlock textBlock2 = new TextBlock();
                textBlock2.Text = mainViewModel.homeModel.NewContent[i].Author;
                textBlock2.Tag = mainViewModel.homeModel.NewContent[i].Uid;
                textBlock2.Style = (Style)FindResource("contentblock");
                DockPanel.SetDock(textBlock2, Dock.Right);
                dockPanel.Children.Add(textBlock2);
                textBlock1.MouseDown += (sender, e) =>
                {
                    TextBlock clickedTextBlock = sender as TextBlock;
                    string info = clickedTextBlock.Tag.ToString();
                    ParentWindow.jump_to_post(info);
                };
                textBlock2.MouseDown += (sender, e) =>
                {
                    TextBlock clickedTextBlock = sender as TextBlock;
                    string info = clickedTextBlock.Tag.ToString();
                    ParentWindow.jump_to_user(info);
                };
                newcontent.Children.Add(dockPanel);
            }
            for (int i = 0; i < mainViewModel.homeModel.HotContent.Count(); i++)
            {
                DockPanel dockPanel = new DockPanel();
                dockPanel.LastChildFill = false;
                TextBlock textBlock1 = new TextBlock();
                textBlock1.Text = mainViewModel.homeModel.HotContent[i].Title;
                textBlock1.Tag = mainViewModel.homeModel.HotContent[i].Pid;
                textBlock1.Style = (Style)FindResource("contentblock");
                DockPanel.SetDock(textBlock1, Dock.Left);
                dockPanel.Children.Add(textBlock1);

                TextBlock textBlock2 = new TextBlock();
                textBlock2.Text = mainViewModel.homeModel.HotContent[i].Author;
                textBlock2.Tag = mainViewModel.homeModel.HotContent[i].Uid;
                textBlock2.Style = (Style)FindResource("contentblock");
                DockPanel.SetDock(textBlock2, Dock.Right);
                dockPanel.Children.Add(textBlock2);
                textBlock1.MouseDown += (sender, e) =>
                {
                    TextBlock clickedTextBlock = sender as TextBlock;
                    string info = clickedTextBlock.Tag.ToString();
                    ParentWindow.jump_to_post(info);
                };
                textBlock2.MouseDown += (sender, e) =>
                {
                    TextBlock clickedTextBlock = sender as TextBlock;
                    string info = clickedTextBlock.Tag.ToString();
                    ParentWindow.jump_to_user(info);
                };
                hotcontent.Children.Add(dockPanel);
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            startAnimation(true);
        }
        private void animationRight(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            startAnimation(true);
            timer.Start();
        }

        private void animationLeft(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            startAnimation(false);
            timer.Start();
        }

        private void startAnimation(bool flag)
        {
            leftButton.IsEnabled = false;
            rightButton.IsEnabled = false;
            DoubleAnimation animationOut = new DoubleAnimation();
            DoubleAnimation animationIn = new DoubleAnimation();
            animationOut.Completed += (s, e) =>
            {
                leftButton.IsEnabled = true;
                rightButton.IsEnabled = true;
            };
            textBlockout.Text = mainViewModel.homeModel.HeadInfo[Currentnum].Text;
            textBlockout.Tag = mainViewModel.homeModel.HeadInfo[Currentnum].Hid;
            textBlockoutauthor.Text = "——" + mainViewModel.homeModel.HeadInfo[Currentnum].Author;
            if (flag)
            {
                animationOut.To = -500;
                animationOut.From = 0;
                animationIn.To = 0;
                animationIn.From = 500;
                Nextnum = (Currentnum + 1) % mainViewModel.headtextnum;
            }
            else
            {
                animationOut.To = 500;
                animationOut.From = 0;
                animationIn.To = 0;
                animationIn.From = -500;
                Nextnum = (Currentnum == 0) ? mainViewModel.headtextnum - 1 : Currentnum - 1;
            }
            textBlockin.Text = mainViewModel.homeModel.HeadInfo[Nextnum].Text;
            textBlockin.Tag = mainViewModel.homeModel.HeadInfo[Nextnum].Hid;
            textBlockinauthor.Text = "——" + mainViewModel.homeModel.HeadInfo[Nextnum].Author;
            animationOut.Duration = TimeSpan.FromSeconds(0.5);
            animationIn.Duration = TimeSpan.FromSeconds(0.5);
            translateTransformout.BeginAnimation(TranslateTransform.XProperty, animationOut);
            translateTransformin.BeginAnimation(TranslateTransform.XProperty, animationIn);
            Currentnum = Nextnum;
        }
    }
}
