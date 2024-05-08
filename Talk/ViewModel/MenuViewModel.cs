using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talk.Model;

namespace Talk.ViewModel
{
    //论坛菜单栏view模型
    class MenuViewModel : Common.NotifyBase
    {
        public MenuModel menuModel { get; set; } = new MenuModel();

        //修改数据库用户签到信息
        public void update_check_in()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    bool isContinuous = false;
                    cmd.Connection = App.conn;
                    cmd.CommandText = "select lastcheck from [user] where uid = @uid";
                    cmd.Parameters.AddWithValue("@uid", menuModel.UserData.Uid);
                    SqlDataReader res = cmd.ExecuteReader();
                    if (res.Read())
                    {
                        //检查是否连续签到
                        if (res.IsDBNull(res.GetOrdinal("lastcheck")) || res.GetDateTime(res.GetOrdinal("lastcheck")).Date == DateTime.Today.AddDays(-1))
                            isContinuous = true;
                    }
                    cmd.Parameters.Clear();
                    res.Close();
                    //更新连续签到天数
                    if (isContinuous)
                        cmd.CommandText = "update [user] set lastcheck = @lastcheck, checkdays = checkdays + 1 where uid = @uid";
                    else
                        cmd.CommandText = "update [user] set lastcheck = @lastcheck, checkdays = 1 where uid = @uid";
                    cmd.Parameters.AddWithValue("@lastcheck", DateTime.Today);
                    cmd.Parameters.AddWithValue("@uid", menuModel.UserData.Uid);
                    cmd.ExecuteNonQuery();
                    menuModel.UserData.Lastcheck = DateTime.Today;
                    if (isContinuous)
                        menuModel.UserData.Checkdays += 1;
                    else
                        menuModel.UserData.Checkdays = 1;
                }
            }
            catch (Exception)
            {
                App.notification.SendNotification("ERROR", "签到失败！");
            }
        }
    }
}
