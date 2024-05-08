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
using Talk.ViewModel;

namespace Talk.View
{
    //管理端题头管理页面下查看题头详情窗口
    public partial class check_head : Window
    {
        public check_head(string examine)
        {
            InitializeComponent();
            //如果已经审核了则折叠审核按钮
            if (examine != "未审核")
            {
                passButton.Visibility = Visibility.Hidden;
                failButton.Visibility = Visibility.Hidden;
            }
        }

        private void Close_Window(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        //按下审核通过按钮触发
        private void Head_Pass(object sender, RoutedEventArgs e)
        {
            Head2ViewModel viewModel = DataContext as Head2ViewModel;
            if (viewModel.DoAudit(true))
                this.Close();
        }

        //按下审核不通过按钮触发
        private void Head_Fail(object sender, RoutedEventArgs e)
        {
            Head2ViewModel viewModel = DataContext as Head2ViewModel;
            if (viewModel.DoAudit(false))
                this.Close();
        }
    }
}
