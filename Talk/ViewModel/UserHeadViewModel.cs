using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talk.Model;

namespace Talk.ViewModel
{
    //用户题头view模型
    class UserHeadViewModel : Common.NotifyBase
    {
        public UserHeadModel userHeadModel { get; set; } = new UserHeadModel();
        public UserHeadViewModel(string uid)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    //加载用户提交的题头数据
                    cmd.Connection = App.conn;
                    cmd.CommandText = "select text, examine from headinfo where author = @authorid";
                    cmd.Parameters.AddWithValue("@authorid", uid);
                    using (SqlDataReader res = cmd.ExecuteReader())
                    {
                        if (res.HasRows)
                        {
                            while (res.Read())
                            {
                                HeadData2 head = new HeadData2
                                {
                                    Text = res["text"].ToString(),
                                    Examine = res["examine"].ToString(),
                                };
                                userHeadModel.UserHeadSubmitList.Add(head);
                            }
                        }
                    }
                    cmd.Parameters.Clear();
                    //加载用户收藏的题头数据
                    cmd.CommandText = "select text, anonymous from collect, headinfo where collect.userid = @userid and collect.headid = headinfo.hid";
                    cmd.Parameters.AddWithValue("@userid", uid);
                    using (SqlDataReader res = cmd.ExecuteReader())
                    {
                        if (res.HasRows)
                        {
                            while (res.Read())
                            {
                                HeadData3 head = new HeadData3
                                {
                                    Text = res["text"].ToString(),
                                    Author = res["anonymous"].ToString(),
                                };
                                userHeadModel.UserHeadCollectList.Add(head);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                App.notification.SendNotification("ERROR", "加载失败：" + ex.Message);
            }
        }
    }
}
