using Microsoft.Win32;
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

namespace Talk
{
    /// <summary>
    /// register.xaml 的交互逻辑
    /// </summary>
    public partial class register : Window
    {
        public register()
        {
            InitializeComponent();
            DataContext = new RegisterViewModel();
        }
        private void textUserName_MouseDown(object sender, MouseButtonEventArgs e)
        {

            txtUserName.Focus();
        }

        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUserName.Text) && txtUserName.Text.Length > 0)
            {
                textUserName.Visibility = Visibility.Collapsed;
            }
            else
            {
                textUserName.Visibility = Visibility.Visible;
            }
        }

        private void textBirthday_MouseDown(object sender, MouseButtonEventArgs e)
        {
            calendar.IsOpen = !calendar.IsOpen;
        }

        private void calDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calDate.SelectedDate.HasValue)
            {
                textBirthday.Text = calDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                calendar.IsOpen = false;
            }
        }
        
        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }

        private void txtPassword_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
            {
                textPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                textPassword.Visibility = Visibility.Visible;
            }
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void close_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void textPassword2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword2.Focus();
        }

        private void txtPassword2_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword2.Password) && txtPassword2.Password.Length > 0)
            {
                textPassword2.Visibility = Visibility.Collapsed;
            }
            else
            {
                textPassword2.Visibility = Visibility.Visible;
            }
        }
    }
}
