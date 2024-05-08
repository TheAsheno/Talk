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
    //注册view模型
    class RegisterViewModel : Common.NotifyBase
    {
        MainWindow window;

        //注册命令
        public RegisterModel RegisterModel { get; set; } = new RegisterModel();
        public Common.CommandBase RegisterCommand { get; set; }
        public Common.CommandBase TextMouseDownCommand { get; set; }

        public string filename = Get_default_filename();

        //默认头像数据
        private static string Get_default_filename()
        {
            DirectoryInfo topDir = Directory.GetParent(Environment.CurrentDirectory);
            string path = topDir.Parent.FullName;
            return path + "\\Asset\\images\\default.png";
        }

        //用户名填写是否合法
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

        //邮箱填写是否合法
        private bool _isEmailError;
        public bool IsEmailError
        {
            get { return _isEmailError; }
            set
            {
                _isEmailError = value;
                DoNotify("IsEmailError");
            }
        }

        //生日填写是否合法
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

        //密码填写是否合法
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

        //确认密码是否和密码一致
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

        //注册
        private void DoRegister(object o)
        {
            //检查各信息是否合法
            if (string.IsNullOrEmpty(RegisterModel.UserName))
            {
                IsUserNameError = true;
                App.notification.SendNotification("ERROR", "请输入用户名！");
                return;
            }
            Regex regex = new Regex("^[a-zA-Z0-9]{6,}$");
            if (!regex.IsMatch(RegisterModel.UserName))
            {
                IsUserNameError = true;
                App.notification.SendNotification("ERROR", "用户名只能由数字、小写字母、大写字母组成，不少于六个字符！");
                return;
            }
            if (string.IsNullOrEmpty(RegisterModel.Email))
            {
                IsEmailError = true;
                App.notification.SendNotification("ERROR", "请输入邮箱！");
                return;
            }
            Regex regex2 = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex2.IsMatch(RegisterModel.Email))
            {
                IsEmailError = true;
                App.notification.SendNotification("ERROR", "邮箱地址不合法！");
                return;
            }
            if (RegisterModel.Birthday == "Birthday")
            {
                IsBirthdayError = true;
                App.notification.SendNotification("ERROR", "请输入生日！");
                return;
            }
            if (string.IsNullOrEmpty(RegisterModel.PassWord))
            {
                IsPassWordError = true;
                App.notification.SendNotification("ERROR", "请输入密码！");
                return;
            }
            if (!regex.IsMatch(RegisterModel.PassWord))
            {
                IsPassWordError = true;
                App.notification.SendNotification("ERROR", "密码只能由数字、小写字母、大写字母组成，不少于六个字符！");
                return;
            }
            if (string.IsNullOrEmpty(RegisterModel.PassWord2))
            {
                IsPassWord2Error = true;
                App.notification.SendNotification("ERROR", "请确认密码！");
                return;
            }
            if (RegisterModel.PassWord != RegisterModel.PassWord2)
            {
                IsPassWordError = true;
                App.notification.SendNotification("ERROR", "密码错误！");
                return;
            }
            //查询是否有同名用户
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT password FROM [user] WHERE username = @username";
            cmd.Parameters.AddWithValue("@username", RegisterModel.UserName);
            cmd.Connection = App.conn;
            SqlDataReader res = cmd.ExecuteReader();
            if (res.HasRows)
            {
                IsUserNameError = true;
                App.notification.SendNotification("ERROR", "该用户名已存在！");
                res.Close();
            }
            else
            {
                res.Close();
                cmd.Parameters.Clear();
                //取出最后的用户编号
                cmd.CommandText = "select top 1 uid from [user] order by uid desc";
                string maxUid = cmd.ExecuteScalar()?.ToString();
                int maxUidNumber = maxUid == null ? 0 : int.Parse(maxUid.Substring(1));
                string newUid = "u" + (++maxUidNumber).ToString("0000");
                cmd.Parameters.Clear();
                //往数据库插入新用户
                cmd.CommandText = "INSERT INTO [user] (uid, username, password, birthday, email, sex, regdate, checkdays, avatar, avatarLastScaleX, avatarLastScaleY, lastCenterPointX, lastCenterPointY, lastX, lastY) VALUES (@uid, @username, @password, @birthday, @email, @sex, @regdate, @checkdays, @avatar, @avatarLastScaleX, @avatarLastScaleY, @lastCenterPointX, @lastCenterPointY, @lastX, @lastY)";
                cmd.Parameters.AddWithValue("@uid", newUid);
                cmd.Parameters.AddWithValue("@username", RegisterModel.UserName);
                cmd.Parameters.AddWithValue("@password", RegisterModel.PassWord);
                cmd.Parameters.AddWithValue("@birthday", RegisterModel.Birthday);
                string mela = "\ue611", female = "\ue60f";
                if (RegisterModel.Sex == mela)
                    cmd.Parameters.AddWithValue("@sex", "男");
                else if (RegisterModel.Sex == female)
                    cmd.Parameters.AddWithValue("@sex", "女");
                cmd.Parameters.AddWithValue("@email", RegisterModel.Email);
                cmd.Parameters.AddWithValue("@regdate", DateTime.Today);
                cmd.Parameters.AddWithValue("@checkdays", 0);
                byte[] imageBytes = File.ReadAllBytes(filename);
                cmd.Parameters.AddWithValue("@avatar", imageBytes);
                cmd.Parameters.AddWithValue("@avatarLastScaleX", RegisterModel.AvatarLastScaleX);
                cmd.Parameters.AddWithValue("@avatarLastScaleY", RegisterModel.AvatarLastScaleY);
                cmd.Parameters.AddWithValue("@lastCenterPointX", RegisterModel.LastCenterPointX);
                cmd.Parameters.AddWithValue("@lastCenterPointY", RegisterModel.LastCenterPointY);
                cmd.Parameters.AddWithValue("@lastX", RegisterModel.LastX);
                cmd.Parameters.AddWithValue("@lastY", RegisterModel.LastY);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    App.notification.SendNotification("SUCCESS", "用户注册成功！");
                    window.jump_to_login();
                }
                else
                {
                    App.notification.SendNotification("ERROR", "用户注册失败，请重试！");
                }
            }
        }
    }
}
