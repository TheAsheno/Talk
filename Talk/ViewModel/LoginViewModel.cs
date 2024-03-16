using System;
using System.Data.SqlClient;
using System.Windows;

namespace Talk
{
    class LoginViewModel : Common.NotifyBase
    {
        public Model.LoginModel LoginModel { get; set; } = new Model.LoginModel();
        public Common.CommandBase LoginCommand { get; set; }

        MainWindow window;

        private string _Message;
        public string Message
        {
            get { return _Message; }
            set
            {
                _Message = value;
                DoNotify("Message");
            }
        }

        private bool _isUserNameError;
        public bool IsUserNameError
        {
            get { return _isUserNameError; }
            set
            {
                _isUserNameError = value;
                DoNotify("IsUserNameError");
            }
        }

        private bool _isPassWordError;
        public bool IsPassWordError
        {
            get { return _isPassWordError; }
            set
            {
                _isPassWordError = value;
                DoNotify("IsPassWordError");
            }
        }

        public LoginViewModel()
        {
            window = (MainWindow)Application.Current.MainWindow;
            LoginCommand = new Common.CommandBase();
            LoginCommand.DoExecute = new Action<object>(DoLogin);
            LoginCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });
        }

        private void DoLogin(object o)
        {
            this.Message = "";
            if (string.IsNullOrEmpty(LoginModel.UserName))
            {
                Message = "请输入用户名！";
                IsUserNameError = true;
                SendNotification("ERROR");
                return;
            }
            if (string.IsNullOrEmpty(LoginModel.PassWord))
            {
                Message = "请输入密码！";
                IsPassWordError = true;
                SendNotification("ERROR");
                return;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT password FROM [user] WHERE username = @username";
            cmd.Parameters.AddWithValue("@username", LoginModel.UserName);
            cmd.Connection = App.conn;
            SqlDataReader res = cmd.ExecuteReader();
            if (res.HasRows)
            {
                while (res.Read())
                {
                    string hashedPasswordFromDB = res.GetString(res.GetOrdinal("password"));
                    if (hashedPasswordFromDB == LoginModel.PassWord)
                    {
                        Message = "登录成功！";
                        SendNotification("SUCCESS");
                        Window home = new View.home_page();
                        window.Close();
                        home.Show();
                    }
                    else
                    {
                        Message = "密码错误！";
                        IsPassWordError = true;
                        SendNotification("ERROR");
                    }
                }
            }
            else
            {
                Message = "该用户不存在！";
                IsUserNameError = true;
                SendNotification("ERROR");
            }
            res.Close();
        }
        private void SendNotification(string title)
        {
            Model.NotificationModel data = new Model.NotificationModel();
            data.Title = title;
            data.Message = Message;
            NotificationWindow dialog = new NotificationWindow();
            dialog.TopFrom = App.GetTopFrom();
            dialog.Closed += Dialog_Closed;
            App._dialogs.Add(dialog);
            dialog.DataContext = data;
            dialog.Show();
        }
        private void Dialog_Closed(object sender, EventArgs e)
        {
            NotificationWindow closedDialog = sender as NotificationWindow;
            App._dialogs.Remove(closedDialog);
        }
    }
}
