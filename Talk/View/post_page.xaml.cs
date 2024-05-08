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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Talk.ViewModel;

namespace Talk.View
{
    //帖子页面
    public partial class post_page : Page
    {
        PostViewModel postViewModel;
        string pid;
        public post_page(string pid, string uid)
        {
            InitializeComponent();
            this.pid = pid;
            postViewModel = new PostViewModel(pid);
            DataContext = postViewModel;
            postViewModel.postModel.UserId = uid;
            displayReply();
            BitmapImage image = ByteArrayToImage(postViewModel.postModel.AuthorInfo.Avatar);
            Avatar.img.Source = image;
            //动态绑定用户头像数据
            TransformGroup transformGroup = Avatar.img.RenderTransform as TransformGroup;
            ScaleTransform sfr = transformGroup.Children[0] as ScaleTransform;
            TranslateTransform tlt = transformGroup.Children[1] as TranslateTransform;
            BindingOperations.SetBinding(sfr, ScaleTransform.ScaleXProperty, new Binding("postModel.AuthorInfo.AvatarLastScaleX"));
            BindingOperations.SetBinding(sfr, ScaleTransform.ScaleYProperty, new Binding("postModel.AuthorInfo.AvatarLastScaleY"));
            BindingOperations.SetBinding(sfr, ScaleTransform.CenterXProperty, new Binding("postModel.AuthorInfo.LastCenterPointX"));
            BindingOperations.SetBinding(sfr, ScaleTransform.CenterYProperty, new Binding("postModel.AuthorInfo.LastCenterPointY"));
            BindingOperations.SetBinding(tlt, TranslateTransform.XProperty, new Binding("postModel.AuthorInfo.LastX"));
            BindingOperations.SetBinding(tlt, TranslateTransform.YProperty, new Binding("postModel.AuthorInfo.LastY"));
        }

        //将字节数组转换为 BitmapImage 对象
        public BitmapImage ByteArrayToImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                return null;

            BitmapImage image = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
            }
            return image;
        }

        //显示回帖
        private void displayReply()
        {
            for (int i = postViewModel.CurrentStart - 1; i < postViewModel.CurrentEnd; i++)
            {
                Border border = new Border();
                border.Margin = new Thickness(10);
                border.BorderThickness = new Thickness(2);
                border.BorderBrush = Brushes.Black;
                Grid grid = new Grid();
                grid.Height = double.NaN;
                grid.Background = Brushes.White;
                DockPanel dockPanel = new DockPanel();
                dockPanel.Margin = new Thickness(10);
                dockPanel.LastChildFill = false;

                TextBlock textBlock1 = new TextBlock();
                textBlock1.Text = postViewModel.postModel.Replys[i].Content;
                textBlock1.FontSize = 13;
                textBlock1.TextWrapping = TextWrapping.Wrap;
                textBlock1.Margin = new Thickness(0, 0, 0, 10);
                DockPanel.SetDock(textBlock1, Dock.Top);

                TextBlock textBlock2 = new TextBlock();
                textBlock2.Text = postViewModel.postModel.Replys[i].Time.ToString("yyyy-MM-dd HH:mm");
                textBlock2.HorizontalAlignment = HorizontalAlignment.Right;
                textBlock2.Style = (Style)Application.Current.FindResource("textHint");
                textBlock2.FontSize = 10;
                DockPanel.SetDock(textBlock2, Dock.Bottom);

                TextBlock textBlock3 = new TextBlock();
                textBlock3.Text = postViewModel.postModel.Replys[i].Author;
                textBlock3.HorizontalAlignment = HorizontalAlignment.Right;
                textBlock3.Style = (Style)Application.Current.FindResource("textHint");
                textBlock3.FontSize = 10;
                DockPanel.SetDock(textBlock3, Dock.Bottom);

                dockPanel.Children.Add(textBlock1);
                dockPanel.Children.Add(textBlock2);
                dockPanel.Children.Add(textBlock3);
                grid.Children.Add(dockPanel);
                border.Child = grid;
                page.Children.Add(border);
            }
        }
        private home _parentWin;
        public home ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtReply.Text) && txtReply.Text.Length > 0)
            {
                textReply.Visibility = Visibility.Collapsed;
            }
            else
            {
                textReply.Visibility = Visibility.Visible;
            }
        }

        //下一页
        private void Next_Page(object sender, RoutedEventArgs e)
        {
            postViewModel.loadReply();
            postViewModel.Refresh();
            if (postViewModel.DoNext())
            {
                ParentWindow.scroll_to_top();
            }
            page.Children.Clear();
            page.Children.Add(title);
            if (postViewModel.CurrentPage == 1)
            {
                page.Children.Add(content);
            }
            displayReply();
        }

        //上一页
        private void Prev_Page(object sender, RoutedEventArgs e)
        {
            postViewModel.loadReply();
            postViewModel.Refresh();
            if (postViewModel.DoPrev())
            {
                ParentWindow.scroll_to_top();
            }
            page.Children.Clear();
            page.Children.Add(title);
            if (postViewModel.CurrentPage == 1)
            {
                page.Children.Add(content);
            }
            displayReply();
        }

        //回帖
        private void Reply(object sender, RoutedEventArgs e)
        {
            //如果回帖成功
            if (postViewModel.DoReply())
            {
                postViewModel.loadReply(); //重新加载回帖列表
                postViewModel.Refresh(); //刷新变量
                postViewModel.ReplyJustify(); //调整变量
                page.Children.Clear(); //清空页面原本的回帖
                page.Children.Add(title); //加上标题元素
                if (postViewModel.CurrentPage == 1)
                {
                    //如果当前是第一页则加上帖子首楼内容
                    page.Children.Add(content);
                }
                displayReply(); //动态显示回帖
            }
        }

        //刷新帖子
        private void Post_Refresh(object sender, MouseButtonEventArgs e)
        {
            ParentWindow.jump_to_post(pid);
        }

        //点击作者查看对方主页
        private void ClickAuthor(object sender, MouseButtonEventArgs e)
        {
            ParentWindow.jump_to_user(postViewModel.postModel.PostInfo.AuthorId);
        }
    }
}
