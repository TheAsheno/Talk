using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Talk.Model;

namespace Talk.ViewModel
{
    //管理端用户管理view模型
    class User1ViewModel : Common.NotifyBase
    {
        public User1Model user1Model { get; set; } = new User1Model();

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
        public User1ViewModel()
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
                    //加载用户数据
                    cmd.Connection = App.conn;
                    cmd.CommandText = "select uid, username, password, sex, email, birthday, regdate from [user]";
                    SqlDataReader res = cmd.ExecuteReader();
                    if (res.HasRows)
                    {
                        while (res.Read())
                        {
                            if (res["uid"].ToString() == "u0000")
                                continue;
                            UserData2 userData2 = new UserData2
                            {
                                Uid = res["uid"].ToString(),
                                Username = res["username"].ToString(),
                                Password = res["password"].ToString(),
                                Email = res["email"].ToString(),
                                Sex = res["sex"].ToString(),
                                Birthday = ((DateTime)res["birthday"]).ToString("yyyy-MM-dd"),
                                Regdate = (DateTime)res["regdate"],
                            };
                            user1Model.Users.Add(userData2);
                        }
                    }
                    res.Close();
                }
                TotalCount = user1Model.GetTotalCount;
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
            user1Model.Users.Clear();
            StringBuilder queryBuilder = new StringBuilder("select uid, username, password, sex, email, birthday, regdate from [user]  where 1=1");
            if (!string.IsNullOrEmpty(user1Model.Uid))
            {
                queryBuilder.Append($" AND uid = '{user1Model.Uid}'");
            }
            if (!string.IsNullOrEmpty(user1Model.UserName))
            {
                queryBuilder.Append($" AND username LIKE '%{user1Model.UserName}%'");
            }
            if (!string.IsNullOrEmpty(user1Model.Sex) && user1Model.Sex != "不限")
            {
                queryBuilder.Append($" AND sex = '{user1Model.Sex}'");
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
                            //跳过管理员信息
                            if (res["uid"].ToString() == "u0000")
                                continue;
                            UserData2 userData2 = new UserData2
                            {
                                Uid = res["uid"].ToString(),
                                Username = res["username"].ToString(),
                                Password = res["password"].ToString(),
                                Email = res["email"].ToString(),
                                Sex = res["sex"].ToString(),
                                Birthday = ((DateTime)res["birthday"]).ToString("yyyy-MM-dd"),
                                Regdate = (DateTime)res["regdate"],
                            };
                            user1Model.Users.Add(userData2);
                        }
                    }
                    res.Close();
                }
                TotalCount = user1Model.GetTotalCount;
                PageCount = 5;
                Init();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                App.notification.SendNotification("ERROR", ex.Message);
            }
        }

        //修改用户数据
        private void DoModify(object o)
        {
            //检查各信息是否合法
            UserData2 modifiedUser = user1Model.ModifyInfo;
            if (string.IsNullOrEmpty(modifiedUser.Username))
            {
                App.notification.SendNotification("ERROR", "用户名不能为空！");
                return;
            }
            Regex regex = new Regex("^[a-zA-Z0-9]{6,}$");
            if (!regex.IsMatch(modifiedUser.Username))
            {
                App.notification.SendNotification("ERROR", "用户名只能由数字、小写字母、大写字母组成，不少于六个字符！");
                return;
            }
            if (string.IsNullOrEmpty(modifiedUser.Password))
            {
                App.notification.SendNotification("ERROR", "密码不能为空！");
                return;
            }
            if (!regex.IsMatch(modifiedUser.Password))
            {
                App.notification.SendNotification("ERROR", "密码只能由数字、小写字母、大写字母组成，不少于六个字符！");
                return;
            }
            if (string.IsNullOrEmpty(modifiedUser.Email))
            {
                App.notification.SendNotification("ERROR", "邮箱不能为空！");
                return;
            }
            Regex regex2 = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex2.IsMatch(modifiedUser.Email))
            {
                App.notification.SendNotification("ERROR", "邮箱地址不合法！");
                return;
            }
            if (string.IsNullOrEmpty(modifiedUser.Birthday))
            {
                App.notification.SendNotification("ERROR", "生日不能为空！");
                return;
            }
            
            string dateFormatPattern = @"^\d{4}-\d{2}-\d{2}$";
            if (!Regex.IsMatch(modifiedUser.Birthday.ToString(), dateFormatPattern))
            {
                App.notification.SendNotification("ERROR", "日期格式必须为 yyyy-MM-dd！");
                return;
            }
            DateTime birthday;
            if (!DateTime.TryParse(modifiedUser.Birthday, out birthday) || birthday > DateTime.Today)
            {
                App.notification.SendNotification("ERROR", "生日日期不合法！");
                return;
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    //数据库修改用户信息
                    cmd.Connection = App.conn;
                    cmd.CommandText = "update [user] set username = @username, password = @password, sex = @sex, birthday = @birthday, email = @email where uid = @uid";
                    cmd.Parameters.AddWithValue("@uid", modifiedUser.Uid);
                    cmd.Parameters.AddWithValue("@username", modifiedUser.Username);
                    cmd.Parameters.AddWithValue("@password", modifiedUser.Password);
                    cmd.Parameters.AddWithValue("@sex", modifiedUser.Sex);
                    cmd.Parameters.AddWithValue("@birthday", birthday);
                    cmd.Parameters.AddWithValue("@email", modifiedUser.Email);
                    cmd.ExecuteNonQuery();
                    App.notification.SendNotification("SUCCESS", "修改成功！");
                    //修改列表相应数据的信息
                    int index = user1Model.Users.FindIndex(u => u.Uid == modifiedUser.Uid);
                    if (index != -1)
                    {
                        user1Model.Users[index].Username = modifiedUser.Username;
                        user1Model.Users[index].Password = modifiedUser.Password;
                        user1Model.Users[index].Sex = modifiedUser.Sex;
                        user1Model.Users[index].Birthday = modifiedUser.Birthday;
                        user1Model.Users[index].Email = modifiedUser.Email;
                        Userslist = user1Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                App.notification.SendNotification("ERROR", "修改失败！");
            }
        }

        //删除该用户
        private void DoDelete(object o)
        {
            if (user1Model.SelectedUser == null)
                return;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = App.conn;
                    cmd.CommandText = "delete from [user] where uid = @uid";
                    cmd.Parameters.AddWithValue("@uid", user1Model.SelectedUser.Uid);
                    cmd.ExecuteNonQuery();
                    App.notification.SendNotification("SUCCESS", "删除成功！");
                    user1Model.Users.Remove(user1Model.SelectedUser);
                    user1Model.SelectedUser = null;
                    //调整变量值
                    TotalCount--;
                    //若当前页是最后一页
                    if (CurrentPage == TotalPage)
                    {
                        //若当前页原本就只有一条记录
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
                    //否则只需重新设置总页数就行
                    else
                    {
                        TotalPage = TotalCount / PageCount * PageCount < TotalCount ? TotalCount / PageCount + 1 : TotalCount / PageCount;
                    }
                    //获取当前页下对应的若干条记录
                    Userslist = user1Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                App.notification.SendNotification("ERROR", "删除失败！");
            }
        }

        //总记录数
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

        //一页的记录数
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

        //总页数
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

        //当前页
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

        //当前页第一条记录的索引
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

        //当前页最后一条记录的索引
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

        //存放所有用户的数据
        private List<UserData2> _userslist;

        public List<UserData2> Userslist
        {
            get { return _userslist; }
            set
            {
                _userslist = value;
                DoNotify("Userslist");
            }
        }

        //初始化各变量
        void Init()
        {
            TotalPage = TotalCount / PageCount * PageCount < TotalCount ? TotalCount / PageCount + 1 : TotalCount / PageCount;
            CurrentPage = 1;
            CurrentStart = 1;
            CurrentEnd = PageCount > TotalCount ? TotalCount : PageCount;
            Userslist = user1Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);
        }

        //下一页
        private void DoNext(object o)
        {
            if (CurrentPage < TotalPage)
            {
                CurrentStart = CurrentPage * PageCount + 1;
                CurrentEnd = CurrentStart + PageCount - 1 > TotalCount ? TotalCount : CurrentStart + PageCount - 1;
                CurrentPage++;
                Userslist = user1Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);
            }
        }

        //上一页
        private void DoPrev(object o)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                CurrentEnd = CurrentStart - 1;
                CurrentStart = (CurrentPage - 1) * PageCount + 1;
                Userslist = user1Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);

            }
        }
    }
}
