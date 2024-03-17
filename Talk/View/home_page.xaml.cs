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
using System.Windows.Shapes;
using System.Windows.Threading;
using Talk.ViewModel;

namespace Talk.View
{
    public partial class home_page : Window
    {
        DispatcherTimer timer;

        HomeViewModel homeViewModel;

        int nextnum, currentnum = 0;
        public home_page()
        {
            InitializeComponent();
            homeViewModel = new HomeViewModel();
            DataContext = homeViewModel;
            homeViewModel.loadHeadInfo();
            textBlockout.Text = homeViewModel.homeModel.HeadInfo[currentnum].Text;
            textBlockoutauthor.Text = "——" + homeViewModel.homeModel.HeadInfo[currentnum].Author;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void exit(object sender, RoutedEventArgs e)
        {
            Close();
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
            DoubleAnimation animationOut = new DoubleAnimation();
            DoubleAnimation animationIn = new DoubleAnimation();
            textBlockout.Text = homeViewModel.homeModel.HeadInfo[currentnum].Text;
            textBlockoutauthor.Text = "——" + homeViewModel.homeModel.HeadInfo[currentnum].Author;
            if (flag)
            {
                animationOut.To = -500;
                animationOut.From = 0;
                animationIn.To = 0;
                animationIn.From = 500;
                nextnum = (currentnum + 1) % homeViewModel.headtextnum;
            }
            else
            {
                animationOut.To = 500;
                animationOut.From = 0;
                animationIn.To = 0;
                animationIn.From = -500;
                nextnum = (currentnum == 0) ? homeViewModel.headtextnum - 1 : currentnum - 1;
            }
            textBlockin.Text = homeViewModel.homeModel.HeadInfo[nextnum].Text;
            textBlockinauthor.Text = "——" + homeViewModel.homeModel.HeadInfo[nextnum].Author;
            animationOut.Duration = TimeSpan.FromSeconds(1);
            animationIn.Duration = TimeSpan.FromSeconds(1);
            translateTransformout.BeginAnimation(TranslateTransform.XProperty, animationOut);
            translateTransformin.BeginAnimation(TranslateTransform.XProperty, animationIn);
            currentnum = nextnum;
        }
    }
}
