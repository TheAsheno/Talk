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
    //论坛首页view模型
    class MainViewModel : Common.NotifyBase
    {
        public MainModel homeModel { get; set; } = new MainModel();

        //收藏题头命令
        public Common.CommandBase CollectHead { get; set; }

        public int headtextnum = 0;

        string userid;
        bool IsheadEmpty = true;

        public MainViewModel(string userid)
        {
            this.userid = userid;
            CollectHead = new Common.CommandBase();
            CollectHead.DoExecute = new Action<object>(DoColletHead);
            CollectHead.DoCanExecute = new Func<object, bool>((o) => { return !IsheadEmpty; });
        }

        //往数据库插入新收藏
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

        //加载题头列表
        public bool loadHeadInfo()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT hid, text, [user].username, headinfo.anonymous FROM headinfo, [user] where headinfo.author = [user].uid and headinfo.examine = '通过'";
                    cmd.Connection = App.conn;

                    using (SqlDataReader res = cmd.ExecuteReader())
                    {
                        if (res.HasRows)
                        {
                            IsheadEmpty = false;
                            while (res.Read())
                            {
                                headtextnum++;
                                HeadInfo info = new HeadInfo
                                {
                                    Text = res["text"].ToString(),
                                    Author = res["anonymous"].ToString(),
                                    Hid = res["hid"].ToString()
                                };
                                homeModel.HeadInfo.Add(info);
                            }
                        }
                        else
                        {
                            //若没有题头则置IsheadEmpty为true
                            IsheadEmpty = true;
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                App.notification.SendNotification("ERROR", "加载失败：" + ex.Message);
                return false;
            }
        }

        //加载推荐帖子列表
        public void loadContent()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = App.conn;
                    cmd.CommandText = "select top 10 pid, title, username, uid from post, [user]  where post.author = [user].uid order by time desc";
                    using (SqlDataReader res = cmd.ExecuteReader())
                    {
                        if (res.HasRows)
                        {
                            while (res.Read())
                            {
                                Content info = new Content
                                {
                                    Pid = res["pid"].ToString(),
                                    Title = res["title"].ToString(),
                                    AuthorId = res["uid"].ToString(),
                                    AuthorName = res["username"].ToString(),
                                };
                                homeModel.NewContent.Add(info);
                            }
                        }
                    }
                    cmd.Parameters.Clear();
                    cmd.CommandText = "select top 10 pid, title, username, uid from post, [user]  where post.author = [user].uid order by clickcount desc";
                    using (SqlDataReader res = cmd.ExecuteReader())
                    {
                        if (res.HasRows)
                        {
                            while (res.Read())
                            {
                                Content info = new Content
                                {
                                    Pid = res["pid"].ToString(),
                                    Title = res["title"].ToString(),
                                    AuthorId = res["uid"].ToString(),
                                    AuthorName = res["username"].ToString(),
                                };
                                homeModel.HotContent.Add(info);
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
