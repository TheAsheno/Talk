using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using Talk.Model;

namespace Talk.ViewModel
{
    class LoginViewModel : Common.NotifyBase
    {
        public LoginModel LoginModel { get; set; } = new LoginModel();
        public Common.CommandBase LoginCommand { get; set; }

        MainWindow window;

        public bool isButtonCanExecute = true;

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
            LoginCommand.DoCanExecute = new Func<object, bool>((o) => { return isButtonCanExecute; });
        }

        private async void DoLogin(object o)
        {
            this.Message = "";
            if (string.IsNullOrEmpty(LoginModel.UserName))
            {
                Message = "请输入用户名！";
                IsUserNameError = true;
                App.notification.SendNotification("ERROR", Message);
                return;
            }
            if (string.IsNullOrEmpty(LoginModel.PassWord))
            {
                Message = "请输入密码！";
                IsPassWordError = true;
                App.notification.SendNotification("ERROR", Message);
                return;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [user] WHERE username = @username";
            cmd.Parameters.AddWithValue("@username", LoginModel.UserName);
            cmd.Connection = App.conn;
            SqlDataReader res = cmd.ExecuteReader();
            if (res.HasRows)
            {
                while (res.Read())
                {
                    if (res["password"].ToString() == LoginModel.PassWord)
                    {
                        ExecuteAnimationCommand();
                        isButtonCanExecute = false;
                        Message = "登录成功！";
                        UserData userData = new UserData
                        {
                            Uid = res["uid"].ToString(),
                            Username = res["username"].ToString(),
                            Password = res["password"].ToString(),
                            Email = res["email"].ToString(),
                            Sex = res["sex"].ToString(),
                            Introduce = res["Introduce"].ToString(),
                            Birthday = (DateTime)res["birthday"],
                            Regdate = (DateTime)res["regdate"],
                            Lastcheck = res["lastcheck"] as DateTime?,
                            Lastlog = DateTime.Now,
                            Checkdays = Convert.ToInt32(res["checkdays"]),
                            Avatar = (byte[])res["avatar"],
                            AvatarLastScaleX = Convert.ToSingle(res["avatarLastScaleX"]),
                            AvatarLastScaleY = Convert.ToSingle(res["avatarLastScaleY"]),
                            LastCenterPointX = Convert.ToSingle(res["lastCenterPointX"]),
                            LastCenterPointY = Convert.ToSingle(res["lastCenterPointY"]),
                            LastX = Convert.ToSingle(res["lastX"]),
                            LastY = Convert.ToSingle(res["lastY"]),
                        };
                        cmd.Parameters.Clear();
                        cmd.CommandText = "update [user] set lastlog = @lastlog where uid = @uid";
                        cmd.Parameters.AddWithValue("@lastlog", DateTime.Now);
                        cmd.Parameters.AddWithValue("@uid", res["uid"].ToString());
                        res.Close();
                        cmd.ExecuteNonQuery();
                        Window home = new View.home(userData);
                        await Task.Delay(2000);
                        App.notification.SendNotification("SUCCESS", Message);
                        window.Close();
                        home.Show();
                        return;
                    }
                    else
                    {
                        Message = "密码错误！";
                        IsPassWordError = true;
                        App.notification.SendNotification("ERROR", Message);
                    }
                }
            }
            else
            {
                Message = "该用户不存在！";
                IsUserNameError = true;
                App.notification.SendNotification("ERROR", Message);
            }
            res.Close();
        }
        public event Action ExecuteAnimationRequested;
        public void ExecuteAnimationCommand()
        {
            ExecuteAnimationRequested?.Invoke();
        }
    }
}
