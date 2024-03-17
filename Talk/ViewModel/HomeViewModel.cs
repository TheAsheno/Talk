using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Talk.ViewModel
{
    class HomeViewModel : Common.NotifyBase
    {
        public Model.HomeModel homeModel { get; set; } = new Model.HomeModel();

        MainWindow window;

        public int headtextnum = 0;

        public HomeViewModel()
        {
            window = (MainWindow)Application.Current.MainWindow;
            
        }

        public void loadHeadInfo()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [headinfo]";
            cmd.Connection = App.conn;
            SqlDataReader res = cmd.ExecuteReader();
            if (res.HasRows)
            {
                while (res.Read())
                {
                    headtextnum++;
                    Model.HeadInfo info = new Model.HeadInfo
                    {
                        Text = res["Text"].ToString(),
                        Author = res["Author"].ToString(),
                        Like = Convert.ToInt32(res["Like"])
                    };
                    homeModel.HeadInfo.Add(info);
                }
                res.Close();
            }
        }
    }
}
