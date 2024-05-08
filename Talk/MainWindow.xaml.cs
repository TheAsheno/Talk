using System;
using System.Windows;
using System.Windows.Input;
using Talk.View;

namespace Talk
{
    //初始登录/注册界面窗口
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            login_page login_page = new login_page();
            mainFrame.Content = login_page;
            login_page.ParentWindow = this;
        }       

        private void close_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        //跳转到注册页面
        public void jump_to_register()
        {
            register_page r = new register_page();
            mainFrame.Content = r;
            r.ParentWindow = this;
        }

        //跳转到登录页面
        public void jump_to_login()
        {
            login_page login_page = new login_page();
            mainFrame.Content = login_page;
            login_page.ParentWindow = this;
            if (mainFrame.BackStack != null)
            {
                while (mainFrame.CanGoBack)
                {
                    mainFrame.RemoveBackEntry();
                }
                GC.Collect();
            }
        }

        //回退到上一各页面并清空页面缓存
        public void mainFrame_goback()
        {
            mainFrame.GoBack();
            mainFrame.RemoveBackEntry();
        }
    }
}
