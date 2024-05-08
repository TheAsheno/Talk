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
    //论坛菜单栏页面
    public partial class home : Window
    {
        MenuViewModel menuViewModel;

        //初始化
        public home(UserData userData)
        {
            InitializeComponent();
            main_page main_page = new main_page(userData.Uid);
            mainFrame.Content = main_page;
            main_page.ParentWindow = this;
            menuViewModel = new MenuViewModel();
            DataContext = menuViewModel;
            menuViewModel.menuModel.UserData = userData;
            //若用户当天已经签到则让签到栏失效
            if (userData.Lastcheck == DateTime.Today)
            {
                checkItem.IsEnabled = false;
            }
        }

        private void Home_Loaded(object sender, RoutedEventArgs e)
        {
            loadBar();
        }

        //加载页面时触发顶部滚动条动画效果
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

        //搜索栏输入时折叠提示文本
        private void txtsearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtsearch.Text) && txtsearch.Text.Length > 0)
                textsearch.Visibility = Visibility.Collapsed;
            else
                textsearch.Visibility = Visibility.Visible;
        }

        private void exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            menu.Focus();
        }

        //跳转到帖子页面
        public void jump_to_post(string info)
        {
            loadBar();
            post_page r = new post_page(info, menuViewModel.menuModel.UserData.Uid);
            mainFrame.Content = r;
            r.ParentWindow = this;
            clear();
        }

        //跳转到某版块帖子列表页面
        public void jump_to_postlist(string SectionId, string SectionName)
        {
            loadBar();
            postlist_page p = new postlist_page(SectionId, SectionName);
            mainFrame.Content = p;
            p.ParentWindow = this;
            clear();
        }

        //滚动到顶部
        public void scroll_to_top()
        {
            scroll.ScrollToTop();
        }

        //跳转到用户主页
        public void jump_to_user(string info)
        {
            //不允许查看管理员主页
            if (info == "u0000")
            {
                return;
            }
            loadBar();
            center_page c = new center_page(info);
            mainFrame.Content = c;
            c.ParentWindow = this;
            clear();
        }

        //跳转到添加题头页面
        public void jump_to_addhead()
        {
            loadBar();
            addhead_page a = new addhead_page(menuViewModel.menuModel.UserData.Uid, menuViewModel.menuModel.UserData.Username);
            mainFrame.Content = a;
            a.ParentWindow = this;
            clear();
        }

        //跳转到用户帖子页面
        public void jump_to_userpost(string uid, string username)
        {
            loadBar();
            userpost_page u = new userpost_page(uid, username);
            mainFrame.Content = u;
            u.ParentWindow = this;
            clear();
        }

        //跳转到用户题头页面
        public void jump_to_userhead(string uid, string username)
        {
            loadBar();
            userhead_page u;
            if (uid == menuViewModel.menuModel.UserData.Uid)
                u = new userhead_page(uid, username, true);
            else u = new userhead_page(uid, username, false);
            mainFrame.Content = u;
            u.ParentWindow = this;
            clear();
        }

        //跳转回论坛首页
        private void Talk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            loadBar();
            main_page main_page = new main_page(menuViewModel.menuModel.UserData.Uid);
            mainFrame.Content = main_page;
            main_page.ParentWindow = this;
            clear();
        }

        //跳转到个人中心页面
        private void jump_to_center(object sender, RoutedEventArgs e)
        {
            loadBar();
            center_page c = new center_page(menuViewModel.menuModel.UserData.Uid, menuViewModel.menuModel.UserData);
            mainFrame.Content = c;
            c.ParentWindow = this;
            clear();
        }

        //清空导航缓存
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

        //签到
        private void check_in(object sender, RoutedEventArgs e)
        {
            menuViewModel.update_check_in();
            checkItem.IsEnabled = false;
            check_in check = new check_in();
            //显示签到窗口
            check.Show();
            var story = (Storyboard)check.Resources["Hide"];
            //动画结束时关闭签到窗口
            story.Completed += delegate { check.Close(); };
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3); 
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                story.Begin(check); 
            };
            timer.Start();
        }

        //跳转到发布帖子页面
        private void jump_to_addpost(object sender, RoutedEventArgs e)
        {
            loadBar();
            addpost_page a = new addpost_page(menuViewModel.menuModel.UserData.Uid);
            mainFrame.Content = a;
            a.ParentWindow = this;
            clear();
        }

        //跳转到版块列表页面
        private void jump_to_section(object sender, RoutedEventArgs e)
        {
            loadBar();
            section_page s = new section_page();
            mainFrame.Content = s;
            s.ParentWindow = this;
            clear();
        }

        //在搜索栏按下enter键后跳转到搜索结果页面
        private void txtsearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                loadBar();
                search_page s = new search_page(menuViewModel.menuModel.SearchText);
                mainFrame.Content = s;
                s.ParentWindow = this;
                clear();
            }
        }
    }
}
