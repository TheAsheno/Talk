using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talk.Model;

namespace Talk.ViewModel
{
    //管理端题头管理view模型
    class Head2ViewModel : Common.NotifyBase
    {
        public Head2Model head2Model { get; set; } = new Head2Model();
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

        public Head2ViewModel()
        {
            QueryCommand = new Common.CommandBase();
            QueryCommand.DoExecute = new Action<object>(DoQuery);
            QueryCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });

            DeleteCommand = new Common.CommandBase();
            DeleteCommand.DoExecute = new Action<object>(DoDelete);
            DeleteCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });

            NextCommand = new Common.CommandBase();
            NextCommand.DoExecute = new Action<object>(DoNext);
            NextCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });

            PrevCommand = new Common.CommandBase();
            PrevCommand.DoExecute = new Action<object>(DoPrev);
            PrevCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });

            //加载题头数据
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    head2Model.Heads = new List<HeadData>();
                    cmd.Connection = App.conn;
                    cmd.CommandText = "select * from headinfo";
                    SqlDataReader res = cmd.ExecuteReader();
                    if (res.HasRows)
                    {
                        while (res.Read())
                        {
                            HeadData userData2 = new HeadData
                            {
                                Hid = res["hid"].ToString(),
                                Text = res["text"].ToString(),
                                Author = res["author"].ToString(),
                                Anonymous = res["anonymous"].ToString(),
                                Examine = res["examine"].ToString(),
                                Submit_time = (DateTime)res["submit_time"],
                                Audit_time = res["audit_time"] as DateTime?,
                            };
                            head2Model.Heads.Add(userData2);
                        }
                    }
                    res.Close();
                }
                TotalCount = head2Model.GetTotalCount;
                PageCount = 5;
                Init();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                App.notification.SendNotification("ERROR", "加载失败！");
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

        //一页展示的记录数
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

        //当前页数
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

        //当前页面首条记录的索引
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

        //当前页面最后一条记录的索引
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

        //当前页面展示的记录列表
        private List<HeadData> _headslist;

        public List<HeadData> Headslist
        {
            get { return _headslist; }
            set
            {
                _headslist = value;
                DoNotify("Headslist");
            }
        }

        //初始化各变量以及题头列表
        void Init()
        {
            TotalPage = TotalCount / PageCount * PageCount < TotalCount ? TotalCount / PageCount + 1 : TotalCount / PageCount;
            CurrentPage = 1;
            CurrentStart = 1;
            CurrentEnd = PageCount > TotalCount ? TotalCount : PageCount;
            Headslist = head2Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);
        }

        //查询语句
        private void DoQuery(object o)
        {
            head2Model.Heads = new List<HeadData>();
            StringBuilder queryBuilder = new StringBuilder("select * from headinfo where 1=1");
            if (!string.IsNullOrEmpty(head2Model.Hid))
            {
                queryBuilder.Append($" AND hid = '{head2Model.Hid}'");
            }
            if (!string.IsNullOrEmpty(head2Model.Author))
            {
                queryBuilder.Append($" AND author LIKE '%{head2Model.Author}%'");
            }
            if (!string.IsNullOrEmpty(head2Model.Examine) && head2Model.Examine != "不限")
            {
                queryBuilder.Append($" AND examine = '{head2Model.Examine}'");
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
                            HeadData userData2 = new HeadData
                            {
                                Hid = res["hid"].ToString(),
                                Text = res["text"].ToString(),
                                Author = res["author"].ToString(),
                                Anonymous = res["anonymous"].ToString(),
                                Examine = res["examine"].ToString(),
                                Submit_time = (DateTime)res["submit_time"],
                                Audit_time = res["audit_time"] as DateTime?,
                            };
                            head2Model.Heads.Add(userData2);
                        }
                    }
                    res.Close();
                }
                TotalCount = head2Model.GetTotalCount;
                PageCount = 5;
                Init();
            }
            catch (Exception ex)
            {
                App.notification.SendNotification("ERROR", ex.Message);
            }
        }

        //审核题头，修改题头数据
        public bool DoAudit(bool isPass)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = App.conn;
                    cmd.CommandText = "update headinfo set examine = @examine, audit_time = @audit_time where hid = @hid";
                    cmd.Parameters.AddWithValue("@hid", head2Model.SelectedHead.Hid);
                    string result;
                    if (isPass)
                        result = "通过";
                    else result = "未通过";
                    cmd.Parameters.AddWithValue("@examine", result);
                    cmd.Parameters.AddWithValue("@audit_time", DateTime.Today);
                    cmd.ExecuteNonQuery();
                    App.notification.SendNotification("SUCCESS", "审核成功！");
                    int index = head2Model.Heads.FindIndex(u => u.Hid == head2Model.SelectedHead.Hid);
                    if (index != -1)
                    {
                        head2Model.Heads[index].Examine = result;
                        head2Model.Heads[index].Audit_time = DateTime.Today;
                        Headslist = head2Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                App.notification.SendNotification("ERROR", "修改失败！");
                return false;
            }
        }

        //在数据库删除该题头
        private void DoDelete(object o)
        {
            if (head2Model.SelectedHead == null)
                return;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = App.conn;
                    cmd.CommandText = "delete from headinfo where hid = @hid";
                    cmd.Parameters.AddWithValue("@hid", head2Model.SelectedHead.Hid);
                    cmd.ExecuteNonQuery();
                    App.notification.SendNotification("SUCCESS", "删除成功！");
                    head2Model.Heads.Remove(head2Model.SelectedHead);
                    head2Model.SelectedHead = null;
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
                    Headslist = head2Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);
                }
            }
            catch (Exception)
            {
                App.notification.SendNotification("ERROR", "删除失败！");
            }
        }

        //下一页
        private void DoNext(object o)
        {
            if (CurrentPage < TotalPage)
            {
                CurrentStart = CurrentPage * PageCount + 1;
                CurrentEnd = CurrentStart + PageCount - 1 > TotalCount ? TotalCount : CurrentStart + PageCount - 1;
                CurrentPage++;
                Headslist = head2Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);
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
                Headslist = head2Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);

            }
        }
    }
}
