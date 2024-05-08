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
    //添加题头页面
    public partial class addhead_page : Page
    {
        AddHeadViewModel addHeadViewModel;
        public addhead_page(string uid, string username)
        {
            InitializeComponent();
            addHeadViewModel = new AddHeadViewModel(uid, username);
            DataContext = addHeadViewModel;
        }
        private home _parentWin;
        public home ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }
    }
}
