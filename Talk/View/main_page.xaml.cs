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
    //论坛首页页面
    public partial class main_page : Page
    {
        DispatcherTimer timer;
        MainViewModel mainViewModel;

        //初始化
        public main_page(string userid)
        {
            InitializeComponent();
            mainViewModel = new MainViewModel(userid);
            DataContext = mainViewModel;
            //加载题头
            if (mainViewModel.loadHeadInfo())
            {
                //播放题头动画
                textBlockout.Text = mainViewModel.homeModel.HeadInfo[Currentnum].Text;
                textBlockout.Tag = mainViewModel.homeModel.HeadInfo[Currentnum].Hid;
                textBlockoutauthor.Text = "——" + mainViewModel.homeModel.HeadInfo[Currentnum].Author;
                leftButton.Click += animationLeft;
                rightButton.Click += animationRight;
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(10);
                timer.Tick += Timer_Tick;
                //启动动画计时器
                timer.Start();
            }
            //加载推荐列表
            mainViewModel.loadContent();
            displayContent();
            
        }
        private home _parentWin;
        public home ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        //当前展示的题头索引
        private int _Currentnum = 0;
        public int Currentnum
        {
            get { return _Currentnum; }
            set
            {
                _Currentnum = value;
            }
        }

        //即将切换到的题头索引
        private int _Nextnum = 0;
        public int Nextnum
        {
            get { return _Nextnum; }
            set
            {
                _Nextnum = value;
            }
        }

        //显示推荐帖子
        private void displayContent()
        {
            //最新的帖子
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
                textBlock2.Text = mainViewModel.homeModel.NewContent[i].AuthorName;
                textBlock2.Tag = mainViewModel.homeModel.NewContent[i].AuthorId;
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

            //最热的帖子
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
                textBlock2.Text = mainViewModel.homeModel.HotContent[i].AuthorName;
                textBlock2.Tag = mainViewModel.homeModel.HotContent[i].AuthorId;
                textBlock2.Style = (Style)FindResource("contentblock");
                DockPanel.SetDock(textBlock2, Dock.Right);
                dockPanel.Children.Add(textBlock2);
                //点击帖子名跳转到其对应帖子页面
                textBlock1.MouseDown += (sender, e) =>
                {
                    TextBlock clickedTextBlock = sender as TextBlock;
                    string info = clickedTextBlock.Tag.ToString();
                    ParentWindow.jump_to_post(info);
                };
                //点击作者名跳转到其对应用户主页
                textBlock2.MouseDown += (sender, e) =>
                {
                    TextBlock clickedTextBlock = sender as TextBlock;
                    string info = clickedTextBlock.Tag.ToString();
                    ParentWindow.jump_to_user(info);
                };
                hotcontent.Children.Add(dockPanel);
            }
        }

        //定时切换到下一个题头
        private void Timer_Tick(object sender, EventArgs e)
        {
            startAnimation(true);
        }

        //按下右侧按钮动画方向从右到左
        private void animationRight(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            startAnimation(true);
            timer.Start();
        }

        //按下左侧按钮动画方向从左到右
        private void animationLeft(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            startAnimation(false);
            timer.Start();
        }

        //播放动画
        private void startAnimation(bool flag)
        {
            //播放动画时左右两侧按钮失效
            leftButton.IsEnabled = false;
            rightButton.IsEnabled = false;
            DoubleAnimation animationOut = new DoubleAnimation();
            DoubleAnimation animationIn = new DoubleAnimation();
            //动画结束激活按钮
            animationOut.Completed += (s, e) =>
            {
                leftButton.IsEnabled = true;
                rightButton.IsEnabled = true;
            };
            //textblockout元素离开视野
            textBlockout.Text = mainViewModel.homeModel.HeadInfo[Currentnum].Text;
            textBlockout.Tag = mainViewModel.homeModel.HeadInfo[Currentnum].Hid;
            textBlockoutauthor.Text = "——" + mainViewModel.homeModel.HeadInfo[Currentnum].Author;
            //根据flag选择动画方向
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
            //textblockout元素进入视野
            textBlockin.Text = mainViewModel.homeModel.HeadInfo[Nextnum].Text;
            textBlockin.Tag = mainViewModel.homeModel.HeadInfo[Nextnum].Hid;
            textBlockinauthor.Text = "——" + mainViewModel.homeModel.HeadInfo[Nextnum].Author;
            animationOut.Duration = TimeSpan.FromSeconds(0.5);
            animationIn.Duration = TimeSpan.FromSeconds(0.5);
            translateTransformout.BeginAnimation(TranslateTransform.XProperty, animationOut);
            translateTransformin.BeginAnimation(TranslateTransform.XProperty, animationIn);
            //更新当前索引
            Currentnum = Nextnum;
        }

        //跳转到添加题头页面
        private void Add_Head(object sender, RoutedEventArgs e)
        {
            ParentWindow.jump_to_addhead();
        }
    }
}
