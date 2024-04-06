using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talk.Model;

namespace Talk.ViewModel
{
    class User1ViewModel : Common.NotifyBase
    {
        public User1Model user1Model { get; set; } = new User1Model();
        public Common.CommandBase QueryCommand { get; set; }
        public Common.CommandBase NextCommand { get; set; }
        public Common.CommandBase PrevCommand { get; set; }
        public User1ViewModel()
        {
            QueryCommand = new Common.CommandBase();
            QueryCommand.DoExecute = new Action<object>(DoQuery);
            QueryCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });
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
                    user1Model.Users = new List<UserData2>();
                    cmd.Connection = App.conn;
                    cmd.CommandText = "select uid, username, password, sex, email, birthday from [user]";
                    SqlDataReader res = cmd.ExecuteReader();
                    if (res.HasRows)
                    {
                        while (res.Read())
                        {
                            UserData2 userData2 = new UserData2
                            {
                                Uid = res["uid"].ToString(),
                                Username = res["username"].ToString(),
                                Password = res["password"].ToString(),
                                Email = res["email"].ToString(),
                                Sex = res["sex"].ToString(),
                                Birthday = (DateTime)res["birthday"],
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
            catch (Exception)
            {
                App.notification.SendNotification("ERROR", "加载失败！");
            }
        }
        private void DoQuery(object o)
        {
            user1Model.Users = new List<UserData2>();
            StringBuilder queryBuilder = new StringBuilder("SELECT * FROM [user] WHERE 1=1");
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
                            UserData2 userData2 = new UserData2
                            {
                                Uid = res["uid"].ToString(),
                                Username = res["username"].ToString(),
                                Password = res["password"].ToString(),
                                Email = res["email"].ToString(),
                                Sex = res["sex"].ToString(),
                                Birthday = (DateTime)res["birthday"],
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
                App.notification.SendNotification("ERROR", ex.Message);
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
        void Init()
        {
            TotalPage = TotalCount / PageCount * PageCount < TotalCount ? TotalCount / PageCount + 1 : TotalCount / PageCount;
            CurrentPage = 1;
            CurrentStart = 1;
            CurrentEnd = PageCount > TotalCount ? TotalCount : PageCount;
            Userslist = user1Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);
        }
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
