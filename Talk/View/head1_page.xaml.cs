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
    //管理端添加题头页面
    public partial class head1_page : Page
    {
        Head1ViewModel head1ViewModel;
        public head1_page()
        {
            InitializeComponent();
            head1ViewModel = new Head1ViewModel();
            DataContext = head1ViewModel;
        }
        private administer _parentWin;
        public administer ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }
    }
}
