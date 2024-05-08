using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Talk.Model;

namespace Talk.ViewModel
{
    //管理端帖子管理view模型
    class Post2ViewModel : Common.NotifyBase
    {
        public Post2Model post2Model { get; set; } = new Post2Model();

        //查询命令
        public Common.CommandBase QueryCommand { get; set; }
        //修改命令
        public Common.CommandBase ModifyCommand { get; set; }
        //删除命令
        public Common.CommandBase DeleteCommand { get; set; }
        //跳转下一页命令
        public Common.CommandBase NextCommand { get; set; }
        //跳转上一页命令
        public Common.CommandBase PrevCommand { get; set; }
        public Post2ViewModel()
        {
            QueryCommand = new Common.CommandBase();
            QueryCommand.DoExecute = new Action<object>(DoQuery);
            QueryCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });

            ModifyCommand = new Common.CommandBase();
            ModifyCommand.DoExecute = new Action<object>(DoModify);
            ModifyCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });

            DeleteCommand = new Common.CommandBase();
            DeleteCommand.DoExecute = new Action<object>(DoDelete);
            DeleteCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });

            NextCommand = new Common.CommandBase();
            NextCommand.DoExecute = new Action<object>(DoNext);
            NextCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });

            PrevCommand = new Common.CommandBase();
            PrevCommand.DoExecute = new Action<object>(DoPrev);
            PrevCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    //加载版块信息
                    cmd.Connection = App.conn;
                    cmd.CommandText = "select sid, name from section";

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
                                post2Model.Sections.Add(section);
                            }
                        }
                    }
                    cmd.Parameters.Clear();
                    //加载帖子列表
                    cmd.CommandText = "select pid, author, title, time, post.clickcount, section.name from post, section where post.section = section.sid";
                    using (SqlDataReader res = cmd.ExecuteReader())
                    {
                        if (res.HasRows)
                        {
                            while (res.Read())
                            {
                                Post post = new Post
                                {
                                    Pid = res["pid"].ToString(),
                                    Section = res["name"].ToString(),
                                    AuthorId = res["author"].ToString(),
                                    Title = res["title"].ToString(),
                                    Time = (DateTime)res["time"],
                                    ClickCount = Convert.ToInt32(res["clickcount"]),
                                };
                                post2Model.Posts.Add(post);
                            }
                        }
                    }
                }
                TotalCount = post2Model.GetTotalCount;
                PageCount = 5;
                Init();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                App.notification.SendNotification("ERROR", "加载失败！");
            }
        }

        //条件查询
        private void DoQuery(object o)
        {
            post2Model.Posts.Clear();
            StringBuilder queryBuilder = new StringBuilder("select pid, author, title, time, post.clickcount, section.name from post, section where post.section = section.sid");
            if (!string.IsNullOrEmpty(post2Model.Pid))
            {
                queryBuilder.Append($" AND pid = '{post2Model.Pid}'");
            }
            if (!string.IsNullOrEmpty(post2Model.PostTitle))
            {
                queryBuilder.Append($" AND title LIKE '%{post2Model.PostTitle}%'");
            }
            if (!string.IsNullOrEmpty(post2Model.PostSection))
            {
                queryBuilder.Append($" AND post.section = '{post2Model.PostSection}'");
            }
            string query = queryBuilder.ToString();
            try
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = App.conn;
                    SqlDataReader res = cmd.ExecuteReader();
                    if (res.HasRows)
                    {
                        while (res.Read())
                        {
                            Post post = new Post
                            {
                                Pid = res["pid"].ToString(),
                                Section = res["name"].ToString(),
                                AuthorId = res["author"].ToString(),
                                Title = res["title"].ToString(),
                                Time = (DateTime)res["time"],
                                ClickCount = Convert.ToInt32(res["clickcount"]),
                            };
                            post2Model.Posts.Add(post);
                        }
                    }
                    res.Close();
                }
                TotalCount = post2Model.GetTotalCount;
                PageCount = 5;
                Init();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                App.notification.SendNotification("ERROR", ex.Message);
            }
        }

        //修改帖子数据
        private void DoModify(object o)
        {
            ComboBoxItem selectedItem = o as ComboBoxItem;
            Post modifiedPost = post2Model.ModifyInfo;
            if (string.IsNullOrEmpty(modifiedPost.Section))
            {
                App.notification.SendNotification("ERROR", "所属版块不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(modifiedPost.AuthorId))
            {
                App.notification.SendNotification("ERROR", "作者不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(modifiedPost.Title))
            {
                App.notification.SendNotification("ERROR", "标题不能为空！");
                return;
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = App.conn;
                    cmd.CommandText = "update post set section = @section, author = @authorid, title = @title where pid = @pid";
                    cmd.Parameters.AddWithValue("@pid", modifiedPost.Pid);
                    cmd.Parameters.AddWithValue("@section", selectedItem.Tag);
                    cmd.Parameters.AddWithValue("@authorid", modifiedPost.AuthorId);
                    cmd.Parameters.AddWithValue("@title", modifiedPost.Title);
                    cmd.ExecuteNonQuery();
                    App.notification.SendNotification("SUCCESS", "修改成功！");
                    int index = post2Model.Posts.FindIndex(u => u.Pid == modifiedPost.Pid);
                    if (index != -1)
                    {
                        post2Model.Posts[index].Section = modifiedPost.Section;
                        post2Model.Posts[index].AuthorId = modifiedPost.AuthorId;
                        post2Model.Posts[index].Title = modifiedPost.Title;
                        Postslist = post2Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                App.notification.SendNotification("ERROR", "修改失败！");
            }
        }

        //删除选中的帖子
        private void DoDelete(object o)
        {
            if (post2Model.SelectedPost == null)
                return;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = App.conn;
                    cmd.CommandText = "delete from post where pid = @pid";
                    cmd.Parameters.AddWithValue("@pid", post2Model.SelectedPost.Pid);
                    cmd.ExecuteNonQuery();
                    App.notification.SendNotification("SUCCESS", "删除成功！");
                    post2Model.Posts.Remove(post2Model.SelectedPost);
                    post2Model.SelectedPost = null;
                    TotalCount--;
                    if (CurrentPage == TotalPage)
                    {
                        if (CurrentEnd - CurrentStart == 0)
                        {
                            TotalPage--;
                            CurrentPage--;
                            CurrentStart -= PageCount;
                            CurrentEnd--;
                        }
                        else
                        {
                            CurrentEnd--;
                        }
                    }
                    else
                    {
                        TotalPage = TotalCount / PageCount * PageCount < TotalCount ? TotalCount / PageCount + 1 : TotalCount / PageCount;
                    }
                    Postslist = post2Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                App.notification.SendNotification("ERROR", "删除失败！");
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
        private List<Post> _postslist;

        public List<Post> Postslist
        {
            get { return _postslist; }
            set
            {
                _postslist = value;
                DoNotify("Postslist");
            }
        }
        void Init()
        {
            TotalPage = TotalCount / PageCount * PageCount < TotalCount ? TotalCount / PageCount + 1 : TotalCount / PageCount;
            CurrentPage = 1;
            CurrentStart = 1;
            CurrentEnd = PageCount > TotalCount ? TotalCount : PageCount;
            Postslist = post2Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);
        }
        private void DoNext(object o)
        {
            if (CurrentPage < TotalPage)
            {
                CurrentStart = CurrentPage * PageCount + 1;
                CurrentEnd = CurrentStart + PageCount - 1 > TotalCount ? TotalCount : CurrentStart + PageCount - 1;
                CurrentPage++;
                Postslist = post2Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);
            }
        }
        private void DoPrev(object o)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                CurrentEnd = CurrentStart - 1;
                CurrentStart = (CurrentPage - 1) * PageCount + 1;
                Postslist = post2Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);

            }
        }
    }
}
