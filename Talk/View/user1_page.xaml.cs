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
    /// <summary>
    /// user1_page.xaml 的交互逻辑
    /// </summary>
    public partial class user1_page : Page
    {
        User1ViewModel user1ViewModel;
        public user1_page()
        {
            InitializeComponent();
            user1ViewModel = new User1ViewModel();
            DataContext = user1ViewModel;
        }
        private administer _parentWin;
        public administer ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        private void borderEdit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void borderDelete_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
        public class User
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public DateTime Birthday { get; set; }
        }
    }
}
