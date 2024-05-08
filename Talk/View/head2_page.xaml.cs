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
    //管理端题头管理页面
    public partial class head2_page : Page
    {
        Head2ViewModel head2ViewModel;
        public head2_page()
        {
            InitializeComponent();
            head2ViewModel = new Head2ViewModel();
            DataContext = head2ViewModel;
        }
        private administer _parentWin;
        public administer ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        //查看选中题头的详情显示新窗口
        private void Check_Head(object sender, RoutedEventArgs e)
        {
            if (head2ViewModel.head2Model.SelectedHead == null)
                return;
            check_head check_Head = new check_head(head2ViewModel.head2Model.SelectedHead.Examine);
            check_Head.DataContext = head2ViewModel;
            check_Head.ShowDialog();
        }
    }
}
