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
    //搜素结果页面
    public partial class search_page : Page
    {
        SearchViewModel searchViewModel;
        public search_page(string info)
        {
            InitializeComponent();
            searchViewModel = new SearchViewModel(info);
            DataContext = searchViewModel;
            title.Text = ">>> 搜索 " + info + " 的结果";
            displaySearchPostlist();
        }

        //动态显示搜索结果
        private void displaySearchPostlist()
        {
            //显示帖子搜索结果
            for (int i = 0; i < searchViewModel.searchModel.SearchPostList.Count(); i++)
            {
                TextBlock textBlock1 = new TextBlock();
                textBlock1.Margin = new Thickness(5);
                textBlock1.Style = (Style)FindResource("contentblock");
                textBlock1.Text = searchViewModel.searchModel.SearchPostList[i].Title;
                textBlock1.Tag = searchViewModel.searchModel.SearchPostList[i].Pid;
                //点击帖子名进入帖子
                textBlock1.MouseDown += (sender, e) =>
                {
                    TextBlock clickedTextBlock = sender as TextBlock;
                    ParentWindow.jump_to_post(clickedTextBlock.Tag.ToString());
                };
                postlike.Children.Add(textBlock1);
            }
            //显示用户搜索结果
            for (int i = 0; i < searchViewModel.searchModel.SearchUserList.Count(); i++)
            {
                TextBlock textBlock2 = new TextBlock();
                textBlock2.Margin = new Thickness(5);
                textBlock2.Style = (Style)FindResource("contentblock");
                textBlock2.Text = searchViewModel.searchModel.SearchUserList[i].UserName;
                textBlock2.Tag = searchViewModel.searchModel.SearchUserList[i].UserId;
                //点击用户名查看对方主页
                textBlock2.MouseDown += (sender, e) =>
                {
                    TextBlock clickedTextBlock = sender as TextBlock;
                    ParentWindow.jump_to_user(clickedTextBlock.Tag.ToString());
                };
                userlike.Children.Add(textBlock2);
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
