using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Talk.Model;

namespace Talk.ViewModel
{
    class MainViewModel : Common.NotifyBase
    {
        public MainModel homeModel { get; set; } = new MainModel();

        public Common.CommandBase AddHead { get; set; }
        public Common.CommandBase CollectHead { get; set; }

        public int headtextnum = 0;

        public string userid;

        public MainViewModel(string userid)
        {
            this.userid = userid;
            CollectHead = new Common.CommandBase();
            CollectHead.DoExecute = new Action<object>(DoColletHead);
            CollectHead.DoCanExecute = new Func<object, bool>((o) => { return true; });
        }

        private void DoColletHead(object o)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "insert into collect (userid, headid) values(@userid, @headid)";
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@headid", o.ToString());
                    cmd.Connection = App.conn;
                    cmd.ExecuteNonQuery();
                    App.notification.SendNotification("SUCCESS", "收藏成功！");
                }
            }
            catch (Exception)
            {
                App.notification.SendNotification("ERROR", "您已收藏过该题头！");
            }
        }

        public void loadHeadInfo()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT hid, text, [user].username, headinfo.anonymous FROM headinfo, [user] where headinfo.author = [user].uid and headinfo.examine = 1";
                    cmd.Connection = App.conn;

                    using (SqlDataReader res = cmd.ExecuteReader())
                    {
                        if (res.HasRows)
                        {
                            while (res.Read())
                            {
                                string author;
                                headtextnum++;
                                if (res["anonymous"] == DBNull.Value)
                                    author = res["username"].ToString();
                                else
                                    author = res["anonymous"].ToString();
                                HeadInfo info = new HeadInfo
                                {
                                    Text = res["text"].ToString(),
                                    Author = author,
                                    Hid = res["hid"].ToString()
                                };
                                homeModel.HeadInfo.Add(info);
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

        public void loadContent()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT content.pid, content.part, post.title, [user].username, [user].uid, post.time FROM content, post, [user] where content.pid = post.pid and post.author = [user].uid";
                    cmd.Connection = App.conn;

                    using (SqlDataReader res = cmd.ExecuteReader())
                    {
                        if (res.HasRows)
                        {
                            while (res.Read())
                            {
                                Content info = new Content
                                {
                                    Title = res["title"].ToString(),
                                    Author = res["username"].ToString(),
                                    Time = res.GetDateTime(res.GetOrdinal("time")),
                                    Pid = res["pid"].ToString(),
                                    Uid = res["uid"].ToString()
                                };
                                if (res["part"].ToString() == "hot")
                                    homeModel.HotContent.Add(info);
                                else if (res["part"].ToString() == "new")
                                    homeModel.NewContent.Add(info);
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
