using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talk.Model;

namespace Talk.ViewModel
{
    class CenterViewModel : Common.NotifyBase
    {
        public CenterModel centerModel { get; set; } = new CenterModel();

        public void load_userinfo(string uid)
        {

        }

        public void save_introduce()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "update [user] set introduce = @introduce where uid = @uid";
                    cmd.Parameters.AddWithValue("@uid", centerModel.UserData.Uid);
                    cmd.Parameters.AddWithValue("@introduce", centerModel.UserData.Introduce);
                    cmd.Connection = App.conn;
                    cmd.ExecuteNonQuery();
                    App.notification.SendNotification("SUCCESS", "编辑成功！");
                }
            }
            catch (Exception)
            {
                App.notification.SendNotification("ERROR", "编辑失败！");
            }
        }
    }
}
