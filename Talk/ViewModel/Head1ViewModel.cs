using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.ViewModel
{
    //管理端添加题头view模型
    class Head1ViewModel : Common.NotifyBase
    {
        //添加题头命令
        public Common.CommandBase AddCommand { get; set; }

        public Head1ViewModel()
        {
            AddCommand = new Common.CommandBase();
            AddCommand.DoExecute = new Action<object>(DoAdd);
            AddCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });
        }

        //往数据库插入新题头
        private void DoAdd(object o)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = App.conn;
                    cmd.CommandText = "select top 1 hid from headinfo order by hid desc";
                    string maxHid = cmd.ExecuteScalar()?.ToString();
                    int maxHidNumber = maxHid == null ? 0 : int.Parse(maxHid.Substring(1));
                    string newHid = "h" + (++maxHidNumber).ToString("0000");
                    cmd.Parameters.Clear();

                    cmd.CommandText = "insert into headinfo (hid, text, author, anonymous, examine, submit_time, audit_time) values(@hid, @text, 'u0000', '管理员', '通过', @submit_time, @audit_time)";
                    cmd.Parameters.AddWithValue("@hid", newHid);
                    cmd.Parameters.AddWithValue("@text", o.ToString());
                    cmd.Parameters.AddWithValue("@submit_time", DateTime.Now);
                    cmd.Parameters.AddWithValue("@audit_time", DateTime.Now);
                    cmd.ExecuteNonQuery();
                    App.notification.SendNotification("SUCCESS", "提交成功！");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                App.notification.SendNotification("ERROR", "提交失败！");
            }
        }
    }
}
