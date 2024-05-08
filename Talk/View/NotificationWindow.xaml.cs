using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Media;
using System.Windows.Media.Animation;

namespace Talk
{
    //提示框窗口
    public partial class NotificationWindow : Window
    {
        public NotificationWindow()
        {
            InitializeComponent();
            
        }
        public double TopFrom { get; set; }
        private double NotifyTimeSpan = 500;
        
        //关闭提示框
        private void button_click(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = FindResource("hideMe") as Storyboard;
            storyboard.Completed += (o, a) => { this.Close(); };
            storyboard.Begin(this);
        }

        //播放动画
        private void NotificationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            NotificationWindow self = sender as NotificationWindow;
            if (self != null)
            {
                self.UpdateLayout();
                SystemSounds.Asterisk.Play();//播放提示声
                double right = SystemParameters.WorkArea.Right;//工作区最右边的值
                self.Top = self.TopFrom - self.ActualHeight;
                DoubleAnimation animation = new DoubleAnimation();
                animation.Duration = new Duration(TimeSpan.FromMilliseconds(NotifyTimeSpan));//NotifyTimeSpan是自己定义的一个int型变量，用来设置动画的持续时间
                animation.From = right;
                animation.To = right - self.ActualWidth;//设定通知从右往左弹出
                self.BeginAnimation(Window.LeftProperty, animation);//设定动画应用于窗体的Left属性

                Task.Factory.StartNew(delegate
                {
                    int seconds = 5;//通知持续5s后消失
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(seconds));
                    //Invoke到主进程中去执行
                    Invoke(self, delegate
                    {
                        animation = new DoubleAnimation();
                        animation.Duration = new Duration(TimeSpan.FromMilliseconds(NotifyTimeSpan));
                        animation.Completed += (s, a) => { self.Close(); };//动画执行完毕，关闭当前窗体
                        animation.From = right - self.ActualWidth;
                        animation.To = right;//通知从左往右收回
                        self.BeginAnimation(Window.LeftProperty, animation);
                    });
                });
            }
        }
        static void Invoke(Window win, Action a)
        {
            win.Dispatcher.Invoke(a);
        }
    }
}
