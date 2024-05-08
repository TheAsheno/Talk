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
using System.Windows.Shapes;

namespace Talk.View
{
    //管理端窗口
    public partial class administer : Window
    {
        public administer()
        {
            InitializeComponent();
        }

        //折叠/展开下拉框
        private void ExpandItem(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if (item != null)
            {
                item.IsExpanded = !item.IsExpanded;
            }
        }

        //跳转到对应页面
        private void TreeViewItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            TreeViewItem item = sender as TreeViewItem;
            switch (item.Name)
            {
                case "user1":
                    user1_page user1 = new user1_page();
                    mainFrame.Content = user1;
                    user1.ParentWindow = this;
                    break;
                case "head1":
                    head1_page head1 = new head1_page();
                    mainFrame.Content = head1;
                    head1.ParentWindow = this;
                    break;
                case "head2":
                    head2_page head2 = new head2_page();
                    mainFrame.Content = head2;
                    head2.ParentWindow = this;
                    break;
                case "section1":
                    section1_page section1 = new section1_page();
                    mainFrame.Content = section1;
                    section1.ParentWindow = this;
                    break;
                case "section2":
                    section2_page section2 = new section2_page();
                    mainFrame.Content = section2;
                    section2.ParentWindow = this;
                    break;
                case "post1":
                    post1_page post1 = new post1_page();
                    mainFrame.Content = post1;
                    post1.ParentWindow = this;
                    break;
                case "post2":
                    post2_page post2 = new post2_page();
                    mainFrame.Content = post2;
                    post2.ParentWindow = this;
                    break;
                default:
                    break;
            }

        }

        //阻止鼠标双击事件路由
        private void CancelDoubleclick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        //退出
        private void Exit_Window(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
