using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talk.Model;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.IO.Packaging;

namespace Talk.ViewModel
{
    class RegisterViewModel : Common.NotifyBase
    {
        MainWindow window;
        public RegisterModel RegisterModel { get; set; } = new RegisterModel();
        public Common.CommandBase RegisterCommand { get; set; }
        public Common.CommandBase TextMouseDownCommand { get; set; }

        public string filename = Get_default_filename();

        private static string Get_default_filename()
        {
            DirectoryInfo topDir = Directory.GetParent(Environment.CurrentDirectory);
            string path = topDir.Parent.FullName;
            return path + "\\Asset\\images\\default.png";
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

        private bool _isBirthdayError;
        public bool IsBirthdayError
        {
            get { return _isBirthdayError; }
            set
            {
                _isBirthdayError = value;
                DoNotify("IsBirthdayError");
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

        private bool _isPassWord2Error;
        public bool IsPassWord2Error
        {
            get { return _isPassWord2Error; }
            set
            {
                _isPassWord2Error = value;
                DoNotify("IsPassWord2Error");
            }
        }

        public RegisterViewModel()
        {
            window = (MainWindow)Application.Current.MainWindow;
            RegisterCommand = new Common.CommandBase();
            RegisterCommand.DoExecute = new Action<object>(DoRegister);
            RegisterCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });
        }
        private void DoRegister(object o)
        {
            if (string.IsNullOrEmpty(RegisterModel.UserName))
            {
                IsUserNameError = true;
                SendNotification("ERROR", "请输入用户名！");
                return;
            }
            Regex regex = new Regex("^[a-zA-Z0-9]{6,}$");
            if (!regex.IsMatch(RegisterModel.UserName))
            {
                IsUserNameError = true;
                SendNotification("ERROR", "用户名只能由数字、小写字母、大写字母组成，不少于六个字符！");
                return;
            }
            if (RegisterModel.Birthday == "Birthday")
            {
                IsBirthdayError = true;
                SendNotification("ERROR", "请输入生日！");
                return;
            }
            if (string.IsNullOrEmpty(RegisterModel.PassWord))
            {
                IsPassWordError = true;
                SendNotification("ERROR", "请输入密码！");
                return;
            }
            if (!regex.IsMatch(RegisterModel.PassWord))
            {
                IsPassWordError = true;
                SendNotification("ERROR", "密码只能由数字、小写字母、大写字母组成，不少于六个字符！");
                return;
            }
            if (string.IsNullOrEmpty(RegisterModel.PassWord2))
            {
                IsPassWord2Error = true;
                SendNotification("ERROR", "请确认密码！");
                return;
            }
            if (RegisterModel.PassWord != RegisterModel.PassWord2)
            {
                IsPassWordError = true;
                SendNotification("ERROR", "密码错误！");
                return;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT password FROM [user] WHERE username = @username";
            cmd.Parameters.AddWithValue("@username", RegisterModel.UserName);
            cmd.Connection = App.conn;
            SqlDataReader res = cmd.ExecuteReader();
            if (res.HasRows)
            {
                IsUserNameError = true;
                SendNotification("ERROR", "该用户名已存在！");
                res.Close();
            }
            else
            {
                res.Close();
                cmd.Parameters.Clear();
                cmd.CommandText = "INSERT INTO [user] (username, password, birthday, avatar, avatarLastScaleX, avatarLastScaleY, lastCenterPointX, lastCenterPointY) VALUES (@username, @password, @birthday, @avatar, @avatarLastScaleX, @avatarLastScaleY, @lastCenterPointX, @lastCenterPointY)";
                cmd.Parameters.AddWithValue("@username", RegisterModel.UserName);
                cmd.Parameters.AddWithValue("@password", RegisterModel.PassWord);
                cmd.Parameters.AddWithValue("@birthday", RegisterModel.Birthday);
                byte[] imageBytes = File.ReadAllBytes(filename);
                cmd.Parameters.AddWithValue("@avatar", imageBytes);
                cmd.Parameters.AddWithValue("@avatarLastScaleX", RegisterModel.AvatarLastScaleX);
                cmd.Parameters.AddWithValue("@avatarLastScaleY", RegisterModel.AvatarLastScaleY);
                cmd.Parameters.AddWithValue("@lastCenterPointX", RegisterModel.LastCenterPointX);
                cmd.Parameters.AddWithValue("@lastCenterPointY", RegisterModel.LastCenterPointY);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    SendNotification("SUCCESS", "用户注册成功！");
                    window.jump_to_login();
                }
                else
                {
                    SendNotification("ERROR", "用户注册失败，请重试！");
                }
            }
        }
        public void SendNotification(string title, string message)
        {
            NotificationModel data = new NotificationModel();
            data.Title = title;
            data.Message = message;
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
