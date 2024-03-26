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
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Talk.Model;
using Talk.ViewModel;

namespace Talk.View
{
    public partial class home : Window
    {
        MenuViewModel menuViewModel;
        public home(UserData userData)
        {
            InitializeComponent();
            main_page main_page = new main_page(userData.Uid);
            mainFrame.Content = main_page;
            main_page.ParentWindow = this;
            menuViewModel = new MenuViewModel();
            DataContext = menuViewModel;
            menuViewModel.menuModel.UserData = userData;
        }
        private void Home_Loaded(object sender, RoutedEventArgs e)
        {
            loadBar();
        }
        private void loadBar()
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 800;
            animation.Duration = TimeSpan.FromSeconds(0.8);

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, progressBar);
            Storyboard.SetTargetProperty(animation, new PropertyPath(ProgressBar.ValueProperty));

            storyboard.Begin();
        }

        private void txtsearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtsearch.Text) && txtsearch.Text.Length > 0)
            {
                textsearch.Visibility = Visibility.Collapsed;
            }
            else
            {
                textsearch.Visibility = Visibility.Visible;
            }
        }

        private void exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            menu.Focus();
        }

        public void jump_to_post(string info)
        {
            loadBar();
            post_page r = new post_page(info);
            mainFrame.Content = r;
            r.ParentWindow = this;
            clear();
        }
        public void jump_to_user(string info)
        {
            Console.WriteLine(info);
        }

        private void Talk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            loadBar();
            main_page main_page = new main_page(menuViewModel.menuModel.UserData.Uid);
            mainFrame.Content = main_page;
            main_page.ParentWindow = this;
            clear();
        }
        private void clear()
        {
            if (mainFrame.BackStack != null)
            {
                while (mainFrame.CanGoBack)
                {
                    mainFrame.RemoveBackEntry();
                }
                GC.Collect();
            }
        }
        private void check_in(object sender, RoutedEventArgs e)
        {
            if (menuViewModel.menuModel.UserData.Lastcheck == DateTime.Today) 
            {
                menuViewModel.SendNotification("ERROR", "当天已签到！");
                return;
            }
            check_in check = new check_in();
            check.Show();
            var story = (Storyboard)check.Resources["Hide"];
            story.Completed += delegate { check.Close(); };
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3); 
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                story.Begin(check); 
            };
            timer.Start();
            menuViewModel.update_check_in();
        }
    }
}
