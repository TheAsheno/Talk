using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talk.Model;

namespace Talk.ViewModel
{
    //版块view模型
    class SectionViewModel : Common.NotifyBase
    {
        public SectionModel sectionModel { get; set; } = new SectionModel();
        public SectionViewModel()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    //加载版块数据
                    cmd.Connection = App.conn;
                    cmd.CommandText = "SELECT sid, name, master, statement, clickcount, postcount, username FROM section, [user] where section.master = [user].uid";
                    using (SqlDataReader res = cmd.ExecuteReader())
                    {
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
                                sectionModel.Sections.Add(section);
                            }
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
    }
}
