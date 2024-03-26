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
            AddHead = new Common.CommandBase();
            AddHead.DoExecute = new Action<object>(DoAddHead);
            AddHead.DoCanExecute = new Func<object, bool>((o) => { return true; });
            CollectHead = new Common.CommandBase();
            CollectHead.DoExecute = new Action<object>(DoColletHead);
            CollectHead.DoCanExecute = new Func<object, bool>((o) => { return true; });
        }

        private void DoAddHead(object o)
        {
            Console.WriteLine("add");
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
                    SendNotification("SUCCESS", "收藏成功！");
                }
            }
            catch (Exception ex)
            {
                SendNotification("ERROR", "收藏失败：" + ex.Message);
            }
        }

        public void loadHeadInfo()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT hid, text, [user].username FROM headinfo, [user] where headinfo.author = [user].uid";
                    cmd.Connection = App.conn;

                    using (SqlDataReader res = cmd.ExecuteReader())
                    {
                        if (res.HasRows)
                        {
                            while (res.Read())
                            {
                                headtextnum++;
                                HeadInfo info = new HeadInfo
                                {
                                    Text = res["text"].ToString(),
                                    Author = res["username"].ToString(),
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
                SendNotification("ERROR", "加载失败：" + ex.Message);
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
                SendNotification("ERROR", "加载失败：" + ex.Message);
            }
        }
        public void SendNotification(string title, string message)
        {
            NotificationModel data = new NotificationModel();
            data.Title = title;
            data.Message = message;
            NotificationWindow dialog = new NotificationWindow();
            dialog.TopFrom = App.GetTopFrom();
            dialog.Closed += Dialog_Closed;
            App._dialogs.Add(dialog);
            dialog.DataContext = data;
            dialog.Show();
        }
        private void Dialog_Closed(object sender, EventArgs e)
        {
            NotificationWindow closedDialog = sender as NotificationWindow;
            App._dialogs.Remove(closedDialog);
        }
    }
}
