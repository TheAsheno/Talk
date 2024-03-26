using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talk.Model;

namespace Talk.ViewModel
{
    class MenuViewModel : Common.NotifyBase
    {
        public MenuModel menuModel { get; set; } = new MenuModel();
        public Common.CommandBase SearchCommand { get; set; }

        public MenuViewModel()
        {
            SearchCommand = new Common.CommandBase();
            SearchCommand.DoExecute = new Action<object>(DoSearch);
            SearchCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });
        }

        private void DoSearch(object o)
        {
            Console.WriteLine(menuModel.SearchText);
        }

        public void update_check_in()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "update [user] set lastcheck = @lastcheck where uid = @uid";
                    cmd.Parameters.AddWithValue("@lastcheck", DateTime.Today);
                    cmd.Parameters.AddWithValue("@uid", menuModel.UserData.Uid);
                    cmd.Connection = App.conn;
                    cmd.ExecuteNonQuery();
                    menuModel.UserData.Lastcheck = DateTime.Today;
                }
            }
            catch (Exception ex)
            {
                SendNotification("ERROR", "签到失败：" + ex.Message);
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
