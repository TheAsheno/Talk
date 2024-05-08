using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talk.Model;

namespace Talk.ViewModel
{
    //某版块下帖子列表view模型
    class PostlistViewModel : Common.NotifyBase
    {
        public PostlistModel postlistModel { get; set; } = new PostlistModel();
        public PostlistViewModel(string Sectionid, string SectionName)
        {
            try
            {
                postlistModel.SectionName = SectionName;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = App.conn;
                    //更新版块点击数
                    cmd.CommandText = "update section set clickcount = clickcount + 1 where sid = @sid";
                    cmd.Parameters.AddWithValue("@sid", Sectionid);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    //加载帖子列表
                    cmd.CommandText = "SELECT pid, title, username, uid FROM post, [user] where post.section = @sid and [user].uid = post.author order by lastreply desc";
                    cmd.Parameters.AddWithValue("@sid", Sectionid);
                    using (SqlDataReader res = cmd.ExecuteReader())
                    {
                        if (res.HasRows)
                        {
                            while (res.Read())
                            {
                                PostList post = new PostList
                                {
                                    Pid = res["pid"].ToString(),
                                    Title = res["title"].ToString(),
                                    AuthorName = res["username"].ToString(),
                                    AuthorId = res["uid"].ToString(),
                                };
                                postlistModel.PostList.Add(post);
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
