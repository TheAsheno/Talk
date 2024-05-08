using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talk.Model;

namespace Talk.ViewModel
{
    //搜索栏view模型
    class SearchViewModel : Common.NotifyBase
    {
        public SearchModel searchModel { get; set; } = new SearchModel();
        public SearchViewModel(string info)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    //加载帖子搜索结果
                    cmd.Connection = App.conn;
                    cmd.CommandText = "select pid, title FROM post where title like @title";
                    cmd.Parameters.AddWithValue("@title", "%" + info + "%");
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
                                searchModel.SearchPostList.Add(post);
                            }
                        }
                    }
                    cmd.Parameters.Clear();
                    //加载用户搜索结果
                    cmd.CommandText = "select uid, username FROM [user] where username like @username";
                    cmd.Parameters.AddWithValue("@username", "%" + info + "%");
                    using (SqlDataReader res = cmd.ExecuteReader())
                    {
                        if (res.HasRows)
                        {
                            while (res.Read())
                            {
                                SearchAuthor user = new SearchAuthor
                                {
                                    UserId = res["uid"].ToString(),
                                    UserName = res["username"].ToString(),
                                };
                                searchModel.SearchUserList.Add(user);
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
