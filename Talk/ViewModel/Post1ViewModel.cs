using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talk.Model;

namespace Talk.ViewModel
{
    //管理端添加帖子view模型
    class Post1ViewModel : Common.NotifyBase
    {
        public Post1Model post1Model { get; set; } = new Post1Model();

        //添加帖子命令
        public Common.CommandBase AddCommand { get; set; }
        public Post1ViewModel()
        {
            AddCommand = new Common.CommandBase();
            AddCommand.DoExecute = new Action<object>(DoAdd);
            AddCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });
            //加载版块数据
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = App.conn;
                    cmd.CommandText = "select * from section";
                    using (SqlDataReader res = cmd.ExecuteReader())
                    {
                        if (res.HasRows)
                        {
                            while (res.Read())
                            {
                                Section2 section = new Section2
                                {
                                    Sid = res["sid"].ToString(),
                                    Name = res["name"].ToString(),
                                };
                                post1Model.Sections.Add(section);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                App.notification.SendNotification("ERROR", "加载失败！");
            }
        }

        //往数据库插入新帖子
        private void DoAdd(object o)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    //取出最后的帖子编号
                    cmd.Connection = App.conn;
                    cmd.CommandText = "select top 1 pid from post order by pid desc";
                    string maxPid = cmd.ExecuteScalar()?.ToString();
                    int maxPidNumber = maxPid == null ? 0 : int.Parse(maxPid.Substring(1));
                    string newPid = "p" + (++maxPidNumber).ToString("0000");
                    cmd.Parameters.Clear();
                    //插入新帖子
                    cmd.CommandText = "insert into post (pid, section, author, title, [content], time, clickcount, lastclick, lastreply) values(@pid, @sectionid, 'u0000', @title, @content, @time, 1, @lastclick, @lastreply)";
                    cmd.Parameters.AddWithValue("@pid", newPid);
                    cmd.Parameters.AddWithValue("@sectionid", post1Model.PostSection);
                    cmd.Parameters.AddWithValue("@title", post1Model.PostTitle);
                    cmd.Parameters.AddWithValue("@content", post1Model.Content);
                    cmd.Parameters.AddWithValue("@time", DateTime.Now);
                    cmd.Parameters.AddWithValue("@lastclick", DateTime.Now);
                    cmd.Parameters.AddWithValue("@lastreply", DateTime.Now);

                    cmd.ExecuteNonQuery();
                    App.notification.SendNotification("SUCCESS", "添加成功！");
                }
            }
            catch (Exception)
            {
                App.notification.SendNotification("ERROR", "添加失败！");
            }
        }
    }
}
