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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Talk.ViewModel;

namespace Talk.View
{
    //用户发布的帖子页面
    public partial class userpost_page : Page
    {
        UserPostViewModel userPostViewModel;
        public userpost_page(string uid, string username)
        {
            InitializeComponent();
            userPostViewModel = new UserPostViewModel(uid);
            DataContext = userPostViewModel;
            title.Text = ">>> 用户 " + username + " 发布的帖子";
            displayUserPostlist();
        }

        //显示帖子列表
        private void displayUserPostlist()
        {
            for (int i = 0; i < userPostViewModel.userPostModel.UserPostList.Count(); i++)
            {
                TextBlock textBlock1 = new TextBlock();
                textBlock1.Margin = new Thickness(5);
                textBlock1.Style = (Style)FindResource("contentblock");
                textBlock1.Text = userPostViewModel.userPostModel.UserPostList[i].Title;
                textBlock1.Tag = userPostViewModel.userPostModel.UserPostList[i].Pid;
                textBlock1.MouseDown += (sender, e) =>
                {
                    TextBlock clickedTextBlock = sender as TextBlock;
                    ParentWindow.jump_to_post(clickedTextBlock.Tag.ToString());
                };
                userpost.Children.Add(textBlock1);
            }
        }

        private home _parentWin;
        public home ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }
    }
}
