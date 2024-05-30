using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talk.Model;

namespace Talk.ViewModel
{
    //帖子view模型
    class PostViewModel : Common.NotifyBase
    {
        public PostModel postModel { get; set; } = new PostModel();
        string pid;
        public PostViewModel(string pid)
        {
            this.pid = pid;
            loadPost();
            loadReply();
            Init();
        }

        //初始化各变量
        public void Init()
        {
            TotalCount = postModel.Replys == null ? 0 : postModel.Replys.Count();
            TotalCountJustify = TotalCount + 1;
            PageCount = 5;
            TotalPage = TotalCount / PageCount * PageCount < TotalCount ? TotalCount / PageCount + 1 : TotalCount / PageCount;
            TotalPageJustify = TotalPage == 0 ? 1 : TotalPage;
            CurrentPage = 1;
            CurrentStart = 1;
            CurrentEnd = PageCount > TotalCount ? TotalCount : PageCount;
        }

        //重新加载后刷新变量
        public void Refresh()
        {
            TotalCount = postModel.Replys == null ? 0 : postModel.Replys.Count();
            TotalCountJustify = TotalCount + 1;
            TotalPage = TotalCount / PageCount * PageCount < TotalCount ? TotalCount / PageCount + 1 : TotalCount / PageCount;
            TotalPageJustify = TotalPage == 0 ? 1 : TotalPage;
        }

        //调整回帖数，因为发帖时的内容也算一个回帖
        public void ReplyJustify()
        {
            CurrentStart = (TotalPage - 1) * PageCount + 1;
            CurrentEnd = CurrentStart + PageCount - 1 > TotalCount ? TotalCount : CurrentStart + PageCount - 1;
            CurrentPage = TotalPage;
        }

        //下一页
        public bool DoNext()
        {
            if (CurrentPage < TotalPage)
            {
                CurrentStart = CurrentPage * PageCount + 1;
                CurrentEnd = CurrentStart + PageCount - 1 > TotalCount ? TotalCount : CurrentStart + PageCount - 1;
                CurrentPage++;
                return true;
            }
            else return false;
        }

        //上一页
        public bool DoPrev()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                CurrentEnd = CurrentStart - 1;
                CurrentStart = (CurrentPage - 1) * PageCount + 1;
                return true;
            }
            else return false;
        }

        //加载帖子
        public void loadPost()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = App.conn;
                    //更新帖子的点击数和最后点击时间
                    cmd.CommandText = "update post set clickcount = clickcount + 1, lastclick = @lastclick where pid = @pid";
                    cmd.Parameters.AddWithValue("@pid", pid);
                    cmd.Parameters.AddWithValue("@lastclick", DateTime.Now);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    //加载帖子数据
                    cmd.CommandText = "SELECT section.name, author, title, [content], time, post.clickcount, lastclick, lastreply, username, [user].uid, avatar, avatarLastScaleX, avatarLastScaleY, lastCenterPointX, lastCenterPointY, lastX, lastY FROM post, [user], section where post.pid = @pid and post.author = [user].uid and post.section = section.sid";
                    cmd.Parameters.AddWithValue("@pid", pid);
                    using (SqlDataReader res = cmd.ExecuteReader())
                    {
                        if (res.Read())
                        {
                            postModel.PostInfo = new Post
                            {
                                Pid = pid,
                                Section = res["name"].ToString(),
                                AuthorId = res["author"].ToString(),
                                Title = res["title"].ToString(),
                                Time = (DateTime)res["time"],
                                Content = res["content"].ToString(),
                                ClickCount = Convert.ToInt32(res["clickcount"]),
                                LastClick = res["lastclick"] as DateTime?,
                                LastReply = res["lastreply"] as DateTime?,
                            };
                            postModel.AuthorInfo = new UserData3
                            {
                                Username = res["username"].ToString(),
                                Avatar = (byte[])res["avatar"],
                                AvatarLastScaleX = Convert.ToSingle(res["avatarLastScaleX"]),
                                AvatarLastScaleY = Convert.ToSingle(res["avatarLastScaleY"]),
                                LastCenterPointX = Convert.ToSingle(res["lastCenterPointX"]),
                                LastCenterPointY = Convert.ToSingle(res["lastCenterPointY"]),
                                LastX = Convert.ToSingle(res["lastX"]),
                                LastY = Convert.ToSingle(res["lastY"]),
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                App.notification.SendNotification("ERROR", "加载失败：");
            }
        }

        //加载回帖
        public void loadReply()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT rid, time, [content], [user].username, [user].uid FROM reply, [user] where reply.post = @pid and reply.author = [user].uid order by reply.time";
                    cmd.Connection = App.conn;
                    cmd.Parameters.AddWithValue("@pid", pid);
                    postModel.Replys.Clear();
                    using (SqlDataReader res = cmd.ExecuteReader())
                    {
                        if (res.HasRows)
                        {
                            while (res.Read())
                            {
                                Reply reply = new Reply
                                {
                                    Rid = res["rid"].ToString(),
                                    Author = res["username"].ToString(),
                                    AuthorId = res["uid"].ToString(),
                                    Time = res.GetDateTime(res.GetOrdinal("time")),
                                    Content = res["content"].ToString(),
                                };
                                postModel.Replys.Add(reply);
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

        //回帖
        public bool DoReply()
        {
            if (string.IsNullOrEmpty(postModel.ReplyText))
            {
                App.notification.SendNotification("ERROR", "回帖不能为空！");
                return false;
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = App.conn;
                    //取出最后的回帖编号
                    cmd.CommandText = "select top 1 rid from reply order by rid desc";
                    string maxRid = cmd.ExecuteScalar()?.ToString();
                    int maxRidNumber = maxRid == null ? 0 : int.Parse(maxRid.Substring(1));
                    string newRid = "r" + (++maxRidNumber).ToString("0000");
                    cmd.Parameters.Clear();
                    //插入新回帖
                    cmd.CommandText = "insert into reply (rid, post, author, [content], time) values(@rid, @postid, @authorid, @content,  @time)";
                    cmd.Parameters.AddWithValue("@rid", newRid);
                    cmd.Parameters.AddWithValue("@postid", postModel.PostInfo.Pid);
                    cmd.Parameters.AddWithValue("@authorid", postModel.UserId);
                    cmd.Parameters.AddWithValue("@content", postModel.ReplyText);
                    cmd.Parameters.AddWithValue("@time", DateTime.Now);
                    cmd.ExecuteNonQuery();
                    //更新帖子的上一次回帖时间
                    cmd.CommandText = "update post set lastreply = @lastreply where pid = @pid";
                    cmd.Parameters.AddWithValue("@pid", pid);
                    cmd.Parameters.AddWithValue("@lastreply", DateTime.Now);
                    cmd.ExecuteNonQuery();
                    //清空回帖内容
                    postModel.ReplyText = "";
                    App.notification.SendNotification("SUCCESS", "回帖成功！");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                App.notification.SendNotification("ERROR", "回帖失败！");
                return false;
            }
        }

        private int _totalCount;

        public int TotalCount
        {
            get { return _totalCount; }
            set
            {
                _totalCount = value;
                DoNotify("TotalCount");
            }
        }
        private int _totalCountJustify;

        public int TotalCountJustify
        {
            get { return _totalCountJustify; }
            set
            {
                _totalCountJustify = value;
                DoNotify("TotalCountJustify");
            }
        }
        private int _pageCount;

        public int PageCount
        {
            get { return _pageCount; }
            set
            {
                _pageCount = value;
                DoNotify("PageCount");
            }
        }

        private int _totalPage;

        public int TotalPage
        {
            get { return _totalPage; }
            set
            {
                _totalPage = value;
                DoNotify("TotalPage");
            }
        }
        private int _totalPageJustify;

        public int TotalPageJustify
        {
            get { return _totalPageJustify; }
            set
            {
                _totalPageJustify = value;
                DoNotify("TotalPageJustify");
            }
        }

        private int _currentPage;

        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                DoNotify("CurrentPage");
            }
        }

        private int _currentStart;

        public int CurrentStart
        {
            get { return _currentStart; }
            set
            {
                _currentStart = value;
                DoNotify("CurrentStart");
            }
        }

        private int _currentEnd;

        public int CurrentEnd
        {
            get { return _currentEnd; }
            set
            {
                _currentEnd = value;
                DoNotify("CurrentEnd");
            }
        }
    }
}
