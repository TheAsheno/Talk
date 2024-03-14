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

namespace Talk.ViewModel
{
    class RegisterViewModel : Common.NotifyBase
    {
        public RegisterModel RegisterModel { get; set; } = new RegisterModel();
        public Common.CommandBase RegisterCommand { get; set; }
        public Common.CommandBase TextMouseDownCommand { get; set; }

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
        public RegisterViewModel()
        {

            RegisterCommand = new Common.CommandBase();
            RegisterCommand.DoExecute = new Action<object>(DoRegister);
            RegisterCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });
        }
        private void DoRegister(object o)
        {
            Message = "";
            if (string.IsNullOrEmpty(RegisterModel.UserName))
            {
                Message = "请输入用户名！";
                IsUserNameError = true;
                SendNotification("ERROR");
                return;
            }
            else
            {
                Regex regex = new Regex("^[a-zA-Z0-9]{6,}$");
                if (!regex.IsMatch(RegisterModel.UserName))
                {
                    Message = "用户名只能由数字、小写字母、大写字母组成，不少于六个字符！";
                    SendNotification("ERROR");
                    return;
                }
            }
            if (string.IsNullOrEmpty(RegisterModel.PassWord))
            {
                Message = "请输入密码！";
                SendNotification("ERROR");
                return;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT password FROM [user] WHERE username = @username";
            cmd.Parameters.AddWithValue("@username", RegisterModel.UserName);
            cmd.Connection = App.conn;
            SqlDataReader res = cmd.ExecuteReader();
            if (res.HasRows)
            {
                while (res.Read())
                {
                    string hashedPasswordFromDB = res.GetString(res.GetOrdinal("password"));
                    if (hashedPasswordFromDB == RegisterModel.PassWord)
                    {
                        Message = "登录成功！";
                        SendNotification("SUCCESS");
                    }
                    else
                    {
                        Message = "密码错误！";
                        SendNotification("ERROR");
                    }
                }
            }
            else
            {
                Message = "该用户不存在！";
                SendNotification("ERROR");
            }
            res.Close();
        }
        private void SendNotification(string title)
        {
            NotificationModel data = new NotificationModel();
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
        private void Test(object o)
        {
            Console.WriteLine("ok");
        }
    }
}
