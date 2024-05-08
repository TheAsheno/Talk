using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;
using Talk.Common;
using Talk.View;

namespace Talk
{
    public partial class App : Application
    {
        public static SqlConnection conn;
        public static Notification notification = new Notification();
        public App()
        {
            //连接数据库
            string SqlConnectionStatement = "server=10.45.226.151;database=Talk;uid=sa;pwd=252011";
            conn = new SqlConnection(SqlConnectionStatement);
            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                    Console.WriteLine("数据库连接成功");
                else
                    Console.WriteLine("数据库连接失败");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("数据库连接失败: " + ex.Message);
            }
        }

        //存放提示消息记录
        public static List<NotificationWindow> _dialogs = new List<NotificationWindow>();

        //设置提示框高度
        public static double GetTopFrom()
        {
            //屏幕的高度-底部TaskBar的高度。
            double topFrom = SystemParameters.WorkArea.Bottom - 10;
            bool isContinueFind = App._dialogs.Any(o => o.TopFrom == topFrom);

            while (isContinueFind)
            {
                topFrom = topFrom - 110;//此处100是NotifyWindow的高
                isContinueFind = App._dialogs.Any(o => o.TopFrom == topFrom);
            }

            if (topFrom <= 0)
                topFrom = SystemParameters.WorkArea.Bottom - 10;

            return topFrom;
        }
    }
}
