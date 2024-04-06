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
    public partial class register_page : Page
    {
        RegisterViewModel registerViewModel;
        public register_page()
        {
            InitializeComponent();
            registerViewModel = new RegisterViewModel();
            DataContext = registerViewModel;
            Avatar.DataContext = registerViewModel;
            Avatar.binding_order();
            TransformGroup transformGroup = Avatar.img.RenderTransform as TransformGroup;
            ScaleTransform sfr = transformGroup.Children[0] as ScaleTransform;
            TranslateTransform tlt = transformGroup.Children[1] as TranslateTransform;
            BindingOperations.SetBinding(sfr, ScaleTransform.ScaleXProperty, new Binding("RegisterModel.AvatarLastScaleX") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Mode = BindingMode.TwoWay });
            BindingOperations.SetBinding(sfr, ScaleTransform.ScaleYProperty, new Binding("RegisterModel.AvatarLastScaleY") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Mode = BindingMode.TwoWay });
            BindingOperations.SetBinding(sfr, ScaleTransform.CenterXProperty, new Binding("RegisterModel.LastCenterPointX") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Mode = BindingMode.TwoWay });
            BindingOperations.SetBinding(sfr, ScaleTransform.CenterYProperty, new Binding("RegisterModel.LastCenterPointY") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Mode = BindingMode.TwoWay });
            BindingOperations.SetBinding(tlt, TranslateTransform.XProperty, new Binding("RegisterModel.LastX") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Mode = BindingMode.TwoWay });
            BindingOperations.SetBinding(tlt, TranslateTransform.YProperty, new Binding("RegisterModel.LastY") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Mode = BindingMode.TwoWay });
        }

        private MainWindow _parentWin;
        public MainWindow ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        private void frame_goback(object sender, MouseButtonEventArgs e)
        {
            ParentWindow.mainFrame_goback();
        }
        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            registerViewModel.IsUserNameError = false;
            if (!string.IsNullOrEmpty(txtUserName.Text) && txtUserName.Text.Length > 0)
            {
                textUserName.Visibility = Visibility.Collapsed;
            }
            else
            {
                textUserName.Visibility = Visibility.Visible;
            }
        }
        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            registerViewModel.IsEmailError = false;
            if (!string.IsNullOrEmpty(txtEmail.Text) && txtEmail.Text.Length > 0)
            {
                textEmail.Visibility = Visibility.Collapsed;
            }
            else
            {
                textEmail.Visibility = Visibility.Visible;
            }
        }

        private void textBirthday_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            rightborder.Focus();
            calendar.IsOpen = !calendar.IsOpen;
        }

        private void calDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calDate.SelectedDate.HasValue)
            {
                if (calDate.SelectedDate > DateTime.Today)
                    App.notification.SendNotification("ERROR", "日期错误！");
                else
                {
                    registerViewModel.IsBirthdayError = false;
                    textBirthday.Text = calDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
                calendar.IsOpen = false;
            }
        }

        private void txtPassword_TextChanged(object sender, RoutedEventArgs e)
        {
            registerViewModel.IsPassWordError = false;
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
                calendar.IsOpen = false;
                rightborder.Focus();
                Window window = Window.GetWindow(this);
                if (window != null)
                {
                    window.DragMove();
                }
            }
        }

        private void txtPassword2_TextChanged(object sender, RoutedEventArgs e)
        {
            registerViewModel.IsPassWord2Error = false;
            if (!string.IsNullOrEmpty(txtPassword2.Password) && txtPassword2.Password.Length > 0)
            {
                textPassword2.Visibility = Visibility.Collapsed;
            }
            else
            {
                textPassword2.Visibility = Visibility.Visible;
            }
        }

        private void close_calendar(object sender, MouseButtonEventArgs e)
        {
            calendar.IsOpen = false;
        }
        string mela = "\ue611", female = "\ue60f";
        private void Change_Sex(object sender, MouseButtonEventArgs e)
        {
            if (Sex.Text == mela)
            {
                Sex.Text = female;
                Sex.FontSize = 20;
            }
            else if (Sex.Text == female)
            {
                Sex.Text = mela;
                Sex.FontSize = 18;
            }
        }
    }
}
