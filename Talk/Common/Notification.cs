using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talk.Model;

namespace Talk.Common
{
    public class Notification
    {
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
