using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talk.Model;

namespace Talk.ViewModel
{
    //发布帖子view模型
    class AddPostViewModel : Common.NotifyBase
    {
        string uid;
        public AddPostModel addPostModel { get; set; } = new AddPostModel();
        public AddPostViewModel(string uid)
        {
            this.uid = uid;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = App.conn;
                    cmd.CommandText = "select sid, name from section";
                    SqlDataReader res = cmd.ExecuteReader();
                    if (res.HasRows)
                    {
                        while (res.Read())
                        {
                            Section2 section = new Section2
                            {
                                Sid = res["sid"].ToString(),
                                Name = res["name"].ToString(),
                            };
                            addPostModel.Sections.Add(section);
                        }
                    }
                    res.Close();
                }
            }
            catch (Exception)
            {
                App.notification.SendNotification("ERROR", "加载失败！");
            }
        }

        //添加帖子
        public bool DoAdd()
        {
            //检查信息是否合法
            if (string.IsNullOrEmpty(addPostModel.PostSection))
            {
                App.notification.SendNotification("ERROR", "请选择帖子所属板块！");
                return false;
            }
            if (string.IsNullOrEmpty(addPostModel.PostTitle))
            {
                App.notification.SendNotification("ERROR", "请填写帖子标题！");
                return false;
            }
            if (string.IsNullOrEmpty(addPostModel.PostContent))
            {
                App.notification.SendNotification("ERROR", "请编辑帖子内容！");
                return false;
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = App.conn;
                    //取出最后的帖子编号
                    cmd.CommandText = "select top 1 pid from post order by pid desc";
                    string maxPid = cmd.ExecuteScalar()?.ToString();
                    int maxPidNumber = maxPid == null ? 0 : int.Parse(maxPid.Substring(1));
                    string newPid = "p" + (++maxPidNumber).ToString("0000");
                    cmd.Parameters.Clear();
                    //往数据库插入新帖子
                    cmd.CommandText = "insert into post (pid, section, author, title, [content], time, clickcount, lastclick, lastreply) values(@pid, @sectionid, @authorid, @title, @content, @time, 1, @lastclick, @lastreply)";
                    cmd.Parameters.AddWithValue("@pid", newPid);
                    cmd.Parameters.AddWithValue("@sectionid", addPostModel.PostSection);
                    cmd.Parameters.AddWithValue("@authorid", uid);
                    cmd.Parameters.AddWithValue("@title", addPostModel.PostTitle);
                    cmd.Parameters.AddWithValue("@content", addPostModel.PostContent);
                    cmd.Parameters.AddWithValue("@time", DateTime.Now);
                    cmd.Parameters.AddWithValue("@lastclick", DateTime.Now);
                    cmd.Parameters.AddWithValue("@lastreply", DateTime.Now);
                    cmd.ExecuteNonQuery();
                    App.notification.SendNotification("SUCCESS", "发布成功！");
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                App.notification.SendNotification("ERROR", "发布失败！");
                return false;
            }
        }
    }
}
