using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talk.Model;

namespace Talk.ViewModel
{
    //管理端添加版块view模型
    class Section1ViewModel : Common.NotifyBase
    {
        public Section1Model section1Model { get; set; } = new Section1Model();

        //添加版块命令
        public Common.CommandBase AddCommand { get; set; }

        public Section1ViewModel()
        {
            AddCommand = new Common.CommandBase();
            AddCommand.DoExecute = new Action<object>(DoAdd);
            AddCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });
        }

        //添加版块
        private void DoAdd(object o)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    //检查是否有同名版块
                    cmd.Connection = App.conn;
                    cmd.CommandText = "select * from section where name = @name";
                    cmd.Parameters.AddWithValue("@name", section1Model.SectionName);
                    using (SqlDataReader res = cmd.ExecuteReader())
                    {
                        if (res.HasRows)
                        {
                            App.notification.SendNotification("ERROR", "已有同名版块！");
                            return;
                        }
                    }
                    cmd.Parameters.Clear();
                    //取出最后的版块编号
                    cmd.CommandText = "select top 1 sid from section order by sid desc";
                    string maxSid = cmd.ExecuteScalar()?.ToString();
                    int maxSidNumber = maxSid == null ? 0 : int.Parse(maxSid.Substring(1));
                    string newSid = "s" + (++maxSidNumber).ToString("0000");
                    cmd.Parameters.Clear();
                    //往数据库插入新版块
                    cmd.CommandText = "insert into section (sid, name, master, statement, clickcount, postcount) values(@sid, @name, @master, @statement, 0, 0)";
                    cmd.Parameters.AddWithValue("@sid", newSid);
                    cmd.Parameters.AddWithValue("@name", section1Model.SectionName);
                    cmd.Parameters.AddWithValue("@master", section1Model.MasterId);
                    cmd.Parameters.AddWithValue("@statement", section1Model.Statement);

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
