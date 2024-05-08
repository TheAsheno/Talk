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
    //某版块下帖子列表页面
    public partial class postlist_page : Page
    {
        PostlistViewModel postlistViewModel;
        public postlist_page(string SectionId, string SectionName)
        {
            InitializeComponent();
            postlistViewModel = new PostlistViewModel(SectionId, SectionName);
            DataContext = postlistViewModel;
            displayPostlist();
        }

        //显示帖子列表
        private void displayPostlist()
        {
            for (int i = 0; i < postlistViewModel.postlistModel.PostList.Count(); i++)
            {
                // 创建新的DockPanel
                DockPanel dockPanel = new DockPanel();
                dockPanel.LastChildFill = false;
                dockPanel.Margin = new Thickness(5);

                // 创建新的TextBlock并添加到DockPanel
                TextBlock textBlock2 = new TextBlock();
                DockPanel.SetDock(textBlock2, Dock.Left);
                textBlock2.Style = (Style)FindResource("contentblock");
                textBlock2.Text = postlistViewModel.postlistModel.PostList[i].Title;
                textBlock2.Tag = postlistViewModel.postlistModel.PostList[i].Pid;
                dockPanel.Children.Add(textBlock2);
                textBlock2.MouseDown += (sender, e) =>
                {
                    TextBlock clickedTextBlock = sender as TextBlock;
                    ParentWindow.jump_to_post(clickedTextBlock.Tag.ToString());
                };

                TextBlock textBlock3 = new TextBlock();
                DockPanel.SetDock(textBlock3, Dock.Right);
                textBlock3.Style = (Style)FindResource("contentblock");
                textBlock3.Text = postlistViewModel.postlistModel.PostList[i].AuthorName;
                textBlock3.Tag = postlistViewModel.postlistModel.PostList[i].AuthorId;
                dockPanel.Children.Add(textBlock3);
                textBlock3.MouseDown += (sender, e) =>
                {
                    TextBlock clickedTextBlock = sender as TextBlock;
                    ParentWindow.jump_to_user(clickedTextBlock.Tag.ToString());
                };

                // 将DockPanel添加到StackPanel
                page.Children.Add(dockPanel);
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
