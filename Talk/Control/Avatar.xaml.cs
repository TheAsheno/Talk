using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Talk.ViewModel;

namespace Talk.Control
{
    /// <summary>
    /// Avatar.xaml 的交互逻辑
    /// </summary>
    public partial class Avatar : UserControl
    {
        public Avatar()
        {
            InitializeComponent();
        }

        //控件的某些特定功能只在特定场合生效，动态绑定
        public void binding_order()
        {
            img.MouseDown += img_MouseDown;
            img.MouseMove += img_MouseMove;
            img.MouseLeave += img_MouseLeave;
            img.MouseUp += img_MouseUp;
            img.MouseWheel += img_MouseWheel;
            change_img.Visibility = Visibility;
            img.Cursor = Cursors.Cross;
        }

        //打开本地文件夹选择图片
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog.ShowDialog() == true)
            {
                img.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                var viewModel = DataContext as RegisterViewModel;
                viewModel.filename = openFileDialog.FileName;
            }
        }

        private bool isMouseLeftButtonDown = false;
        Point previousMousePoint = new Point(0, 0);

        //鼠标按下触发
        private void img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            isMouseLeftButtonDown = true;
            previousMousePoint = e.GetPosition(img);
        }

        //鼠标抬起触发
        private void img_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseLeftButtonDown = false;
        }

        //鼠标从图片上移走触发
        private void img_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseLeftButtonDown = false;
        }

        //鼠标移动时触发，根据isMouseLeftButtonDown的值使仅当鼠标在按住的状态时拖动图片
        private void img_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseLeftButtonDown == true)
            {
                Point position = e.GetPosition(img);
                tlt.X += position.X - this.previousMousePoint.X;
                tlt.Y += position.Y - this.previousMousePoint.Y;
            }
        }

        //滚动鼠标滚轮时触发，对图片进行缩放
        private void img_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point centerPoint = e.GetPosition(img);

            double val = (double)e.Delta / 2000;
            if (sfr.ScaleX < 0.3 && sfr.ScaleY < 0.3 && e.Delta < 0)
            {
                return;
            }
            sfr.CenterX = centerPoint.X;
            sfr.CenterY = centerPoint.Y;

            sfr.ScaleX += val;
            sfr.ScaleY += val;
        }
    }
}
