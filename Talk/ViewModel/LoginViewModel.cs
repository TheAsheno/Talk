using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Talk
{
    class LoginViewModel : Common.NotifyBase
    {
        public Model.LoginModel LoginModel { get; set; } = new Model.LoginModel();
        public Common.CommandBase LoginCommand { get; set; }

        private string _Message;
        public string Message
        {
            get { return _Message; }
            set
            {
                _Message = value;
                this.DoNotify("Message");
            }
        }

        public LoginViewModel()
        {
            this.LoginCommand = new Common.CommandBase();
            this.LoginCommand.DoExecute = new Action<object>(DoLogin);
            this.LoginCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });
        }

        private void DoLogin(object o)
        {
            this.Message = "";
            if (string.IsNullOrEmpty(LoginModel.UserName))
            {
                this.Message = "请输入用户名！";
                SendNotification("ERROR");
                return;
            }
            if (string.IsNullOrEmpty(LoginModel.PassWord))
            {
                this.Message = "请输入密码！";
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
                        this.Message = "登录成功！";
                        SendNotification("SUCCESS");
                    }
                    else
                    {
                        this.Message = "密码错误！";
                        SendNotification("ERROR");
                    }
                }
            }
            else
            {
                this.Message = "该用户不存在！";
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
