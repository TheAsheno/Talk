using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talk.Model;

namespace Talk.ViewModel
{
    //个人中心view模型
    class CenterViewModel : Common.NotifyBase
    {
        public CenterModel centerModel { get; set; } = new CenterModel();

        //加载用户数据
        public void load_userinfo(string uid)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = App.conn;
                    cmd.CommandText = "select * FROM [user] WHERE uid = @uid";
                    cmd.Parameters.AddWithValue("@uid", uid);
                    SqlDataReader res = cmd.ExecuteReader();
                    if (res.Read())
                    {
                        centerModel.UserData = new UserData
                        {
                            Uid = uid,
                            Username = res["username"].ToString(),
                            Email = res["email"].ToString(),
                            Sex = res["sex"].ToString(),
                            Introduce = res["Introduce"].ToString(),
                            Birthday = (DateTime)res["birthday"],
                            Regdate = (DateTime)res["regdate"],
                            Lastcheck = res["lastcheck"] as DateTime?,
                            Lastlog = DateTime.Now,
                            Checkdays = Convert.ToInt32(res["checkdays"]),
                            Avatar = (byte[])res["avatar"],
                            AvatarLastScaleX = Convert.ToSingle(res["avatarLastScaleX"]),
                            AvatarLastScaleY = Convert.ToSingle(res["avatarLastScaleY"]),
                            LastCenterPointX = Convert.ToSingle(res["lastCenterPointX"]),
                            LastCenterPointY = Convert.ToSingle(res["lastCenterPointY"]),
                            LastX = Convert.ToSingle(res["lastX"]),
                            LastY = Convert.ToSingle(res["lastY"]),
                        };
                    }
                    res.Close();
                }
            }
            catch (Exception)
            {
                App.notification.SendNotification("ERROR", "加载失败！");
            }
        }

        //保存编辑后的简介
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
