using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talk.Model;

namespace Talk.ViewModel
{
    //用户帖子view模型
    class UserPostViewModel : Common.NotifyBase
    {
        public UserPostModel userPostModel { get; set; } = new UserPostModel();
        public UserPostViewModel(string uid)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    //加载用户发布的帖子数据
                    cmd.Connection = App.conn;
                    cmd.CommandText = "select pid, title FROM post where author = @authorid";
                    cmd.Parameters.AddWithValue("@authorid", uid);
                    using (SqlDataReader res = cmd.ExecuteReader())
                    {
                        if (res.HasRows)
                        {
                            while (res.Read())
                            {
                                SearchPost post = new SearchPost
                                {
                                    Pid = res["pid"].ToString(),
                                    Title = res["title"].ToString(),
                                };
                                userPostModel.UserPostList.Add(post);
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
