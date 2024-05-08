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
    //用户收藏/提交的题头页面
    public partial class userhead_page : Page
    {
        UserHeadViewModel userHeadViewModel;
        public userhead_page(string uid, string username, bool flag)
        {
            InitializeComponent();
            userHeadViewModel = new UserHeadViewModel(uid);
            DataContext = userHeadViewModel;
            title.Text = ">>> 用户 " + username + " 的题头";
            displayUserHeadlist(flag);
        }

        //显示题头列表
        private void displayUserHeadlist(bool flag)
        {
            if (flag)
            {
                for (int i = 0; i < userHeadViewModel.userHeadModel.UserHeadSubmitList.Count(); i++)
                {
                    TextBlock textBlock1 = new TextBlock();
                    textBlock1.Margin = new Thickness(5, 5, 5, 1);
                    textBlock1.Text = userHeadViewModel.userHeadModel.UserHeadSubmitList[i].Text;
                    submithead.Children.Add(textBlock1);
                    TextBlock textBlock2 = new TextBlock();
                    textBlock2.Margin = new Thickness(5, 1, 5, 5);
                    textBlock2.Text = userHeadViewModel.userHeadModel.UserHeadSubmitList[i].Examine;
                    textBlock2.HorizontalAlignment = HorizontalAlignment.Right;
                    textBlock2.FontSize = 10;
                    submithead.Children.Add(textBlock2);
                }
            }
            else page.Children.Remove(submithead);
            for (int i = 0; i < userHeadViewModel.userHeadModel.UserHeadCollectList.Count(); i++)
            {
                TextBlock textBlock1 = new TextBlock();
                textBlock1.Margin = new Thickness(5);
                textBlock1.Text = userHeadViewModel.userHeadModel.UserHeadCollectList[i].Text;
                collecthead.Children.Add(textBlock1);
                TextBlock textBlock2 = new TextBlock();
                textBlock2.Margin = new Thickness(5, 1, 5, 5);
                textBlock2.Text = userHeadViewModel.userHeadModel.UserHeadCollectList[i].Author;
                textBlock2.HorizontalAlignment = HorizontalAlignment.Right;
                textBlock2.FontSize = 10;
                collecthead.Children.Add(textBlock2);
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
