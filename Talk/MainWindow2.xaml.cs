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

namespace Talk
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow2 : Window
    {
        public MainWindow2()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
            registerbtn.Click += delegate
            {
                var w = new MainWindow
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                w.ShowDialog();
            };
        }
        private void close_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
