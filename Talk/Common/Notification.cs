using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talk.Model;

namespace Talk.Common
{
    //提示框
    public class Notification
    {
        //显示提示框，title分为两类：SUCCESS和ERROR
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
        //移除提示框
        private void Dialog_Closed(object sender, EventArgs e)
        {
            NotificationWindow closedDialog = sender as NotificationWindow;
            App._dialogs.Remove(closedDialog);
        }
    }
}
