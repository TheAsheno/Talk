using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talk.Model;

namespace Talk.ViewModel
{
    //管理端版块管理view模型
    class Section2ViewModel : Common.NotifyBase
    {
        public Section2Model section2Model { get; set; } = new Section2Model();

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
        public Section2ViewModel()
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
            //加载版块数据
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = App.conn;
                    cmd.CommandText = "select sid, name, master, statement, clickcount, postcount, username from section, [user] where section.master = [user].uid";
                    SqlDataReader res = cmd.ExecuteReader();
                    if (res.HasRows)
                    {
                        while (res.Read())
                        {
                            Section section = new Section
                            {
                                Sid = res["sid"].ToString(),
                                Name = res["name"].ToString(),
                                MasterId = res["master"].ToString(),
                                MasterName = res["username"].ToString(),
                                Statement = res["statement"].ToString(),
                                ClickCount = Convert.ToInt32(res["clickcount"]),
                                PostCount = Convert.ToInt32(res["postcount"]),
                            };
                            section2Model.Sections.Add(section);
                        }
                    }
                    res.Close();
                }
                TotalCount = section2Model.GetTotalCount;
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
            section2Model.Sections.Clear();
            StringBuilder queryBuilder = new StringBuilder("select sid, name, master, statement, clickcount, postcount, username from section, [user]  where section.master = [user].uid");
            if (!string.IsNullOrEmpty(section2Model.Sid))
            {
                queryBuilder.Append($" AND sid = '{section2Model.Sid}'");
            }
            if (!string.IsNullOrEmpty(section2Model.SectionName))
            {
                queryBuilder.Append($" AND name LIKE '%{section2Model.SectionName}%'");
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
                            Section section = new Section
                            {
                                Sid = res["sid"].ToString(),
                                Name = res["name"].ToString(),
                                MasterId = res["master"].ToString(),
                                MasterName = res["username"].ToString(),
                                Statement = res["statement"].ToString(),
                                ClickCount = Convert.ToInt32(res["clickcount"]),
                                PostCount = Convert.ToInt32(res["postcount"]),
                            };
                            section2Model.Sections.Add(section);
                        }
                    }
                    res.Close();
                }
                TotalCount = section2Model.GetTotalCount;
                PageCount = 5;
                Init();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                App.notification.SendNotification("ERROR", ex.Message);
            }
        }

        //修改版块
        private void DoModify(object o)
        {
            //检查版块信息是否合法
            Section modifiedSection = section2Model.ModifyInfo;
            if (string.IsNullOrEmpty(modifiedSection.Name))
            {
                App.notification.SendNotification("ERROR", "版块名不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(modifiedSection.MasterId))
            {
                App.notification.SendNotification("ERROR", "版主不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(modifiedSection.Statement))
            {
                App.notification.SendNotification("ERROR", "简介不能为空！");
                return;
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    //数据库修改版块信息
                    cmd.Connection = App.conn;
                    cmd.CommandText = "update section set name = @name, master = @master, statement = @statement where sid = @sid";
                    cmd.Parameters.AddWithValue("@sid", modifiedSection.Sid);
                    cmd.Parameters.AddWithValue("@name", modifiedSection.Name);
                    cmd.Parameters.AddWithValue("@master", modifiedSection.MasterId);
                    cmd.Parameters.AddWithValue("@statement", modifiedSection.Statement);
                    cmd.ExecuteNonQuery();
                    App.notification.SendNotification("SUCCESS", "修改成功！");
                    int index = section2Model.Sections.FindIndex(u => u.Sid == modifiedSection.Sid);
                    if (index != -1)
                    {
                        section2Model.Sections[index].Name = modifiedSection.Name;
                        section2Model.Sections[index].MasterId = modifiedSection.MasterId;
                        section2Model.Sections[index].Statement = modifiedSection.Statement;
                        Sectionslist = section2Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                App.notification.SendNotification("ERROR", "修改失败！");
            }
        }

        //删除该版块
        private void DoDelete(object o)
        {
            if (section2Model.SelectedSection == null)
                return;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = App.conn;
                    cmd.CommandText = "delete from section where sid = @sid";
                    cmd.Parameters.AddWithValue("@sid", section2Model.SelectedSection.Sid);
                    cmd.ExecuteNonQuery();
                    App.notification.SendNotification("SUCCESS", "删除成功！");
                    section2Model.Sections.Remove(section2Model.SelectedSection);
                    section2Model.SelectedSection = null;
                    //调整变量值
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
                    Sectionslist = section2Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);
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
        private List<Section> _sectionslist;

        public List<Section> Sectionslist
        {
            get { return _sectionslist; }
            set
            {
                _sectionslist = value;
                DoNotify("Sectionslist");
            }
        }

        //初始化各变量
        void Init()
        {
            TotalPage = TotalCount / PageCount * PageCount < TotalCount ? TotalCount / PageCount + 1 : TotalCount / PageCount;
            CurrentPage = 1;
            CurrentStart = 1;
            CurrentEnd = PageCount > TotalCount ? TotalCount : PageCount;
            Sectionslist = section2Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);
        }

        //下一页
        private void DoNext(object o)
        {
            if (CurrentPage < TotalPage)
            {
                CurrentStart = CurrentPage * PageCount + 1;
                CurrentEnd = CurrentStart + PageCount - 1 > TotalCount ? TotalCount : CurrentStart + PageCount - 1;
                CurrentPage++;
                Sectionslist = section2Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);
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
                Sectionslist = section2Model.GetList(CurrentStart - 1, CurrentEnd - CurrentStart + 1);

            }
        }
    }
}
