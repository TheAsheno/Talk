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
    /// <summary>
    /// administer.xaml 的交互逻辑
    /// </summary>
    public partial class administer : Window
    {
        public administer()
        {
            InitializeComponent();
        }

        private void ExpandItem(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if (item != null)
            {
                item.IsExpanded = !item.IsExpanded;
            }
        }

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

                    break;
                case "head2":

                    break;
                case "section1":

                    break;
                case "section2":

                    break;
                case "post1":

                    break;
                case "post2":

                    break;
                default:
                    break;
            }

        }

        private void CancelDoubleclick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
