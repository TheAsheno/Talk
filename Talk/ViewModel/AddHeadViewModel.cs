using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Talk.Model;
using Talk.View;

namespace Talk.ViewModel
{
    class AddHeadViewModel : Common.NotifyBase
    {
        public AddHeadModel AddHeadModel { get; set; } = new AddHeadModel();
        public Common.CommandBase AddCommand { get; set; }

        string uid;

        public AddHeadViewModel(string uid)
        {
            this.uid = uid;
            AddCommand = new Common.CommandBase();
            AddCommand.DoExecute = new Action<object>(DoAdd);
            AddCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });
        }
        private void DoAdd(object isChecked)
        {
            bool isCheckedValue = (bool)isChecked;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = App.conn;
                    cmd.CommandText = "select top 1 submit_time from headinfo where author = @author order by hid desc";
                    cmd.Parameters.AddWithValue("@author", uid);
                    SqlDataReader res = cmd.ExecuteReader();
                    if (res.Read())
                    {
                        if (res.GetDateTime(res.GetOrdinal("submit_time")).Date == DateTime.Today)
                        {
                            App.notification.SendNotification("ERROR", "当天已提交过题头！");
                            return;
                        }
                    }
                    cmd.Parameters.Clear();
                    res.Close();

                    cmd.CommandText = "select top 1 hid from headinfo order by hid desc";
                    string maxHid = cmd.ExecuteScalar()?.ToString();
                    int maxHidNumber = maxHid == null ? 0 : int.Parse(maxHid.Substring(1));
                    string newHid = "h" + (++maxHidNumber).ToString("0000");
                    cmd.Parameters.Clear();

                    cmd.CommandText = "insert into headinfo (hid, text, author, anonymous, examine, submit_time) values(@hid, @text, @author, @anonymous, @examine, @submit_time)";
                    cmd.Parameters.AddWithValue("@hid", newHid);
                    cmd.Parameters.AddWithValue("@author", uid);
                    cmd.Parameters.AddWithValue("@text", AddHeadModel.HeadText);
                    cmd.Parameters.AddWithValue("@examine", 0);
                    cmd.Parameters.AddWithValue("@submit_time", DateTime.Now);
                    if (isCheckedValue)
                        cmd.Parameters.AddWithValue("@anonymous", AddHeadModel.Author);
                    else
                        cmd.Parameters.AddWithValue("@anonymous", DBNull.Value);
                    cmd.ExecuteNonQuery();
                    App.notification.SendNotification("SUCCESS", "提交成功！");
                }
            }
            catch (Exception)
            {
                App.notification.SendNotification("ERROR", "提交失败！");
            }
        }
    }
}
