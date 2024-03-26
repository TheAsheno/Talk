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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Talk.ViewModel;

namespace Talk.View
{
    public partial class login_page : Page
    {
        LoginViewModel loginViewModel;
        public login_page()
        {
            InitializeComponent();
            loginViewModel = new LoginViewModel();
            DataContext = loginViewModel;
            loginViewModel.ExecuteAnimationRequested += startLoadAnimation;
        }

        private MainWindow _parentWin;
        public MainWindow ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        private void register(object sender, RoutedEventArgs e)
        {
            if (loginViewModel.isButtonCanExecute)
                ParentWindow.jump_to_register();
        }
        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            loginViewModel.IsUserNameError = false;
            if (!string.IsNullOrEmpty(txtUserName.Text) && txtUserName.Text.Length > 0)
            {
                textUserName.Visibility = Visibility.Collapsed;
            }
            else
            {
                textUserName.Visibility = Visibility.Visible;
            }
        }
        private void txtPassword_TextChanged(object sender, RoutedEventArgs e)
        {
            loginViewModel.IsPassWordError = false;
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
                rightborder.Focus();
                Window window = Window.GetWindow(this);
                if (window != null)
                {
                    window.DragMove();
                }
            }
        }
        public void startLoadAnimation()
        {
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromSeconds(1)
            };
            ellipseLoadAnimation.loadingCanvas.BeginAnimation(Canvas.OpacityProperty, fadeInAnimation);
            ellipseLoadAnimation.loadingCanvas.Visibility = Visibility.Visible;
            Storyboard loadingAnimation = (Storyboard)ellipseLoadAnimation.loadingCanvas.Resources["LoadingAnimation"];
            loadingAnimation.Begin();
        }
    }
}
