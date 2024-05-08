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
using Talk.Model;

namespace Talk.View
{
    //个人中心也买你
    public partial class center_page : Page
    {
        CenterViewModel centerViewModel;

        //初始化
        public center_page(string uid, UserData userData = null)
        {
            InitializeComponent();
            centerViewModel = new CenterViewModel();
            DataContext = centerViewModel;
            //有userData说明是查看用户自己的个人中心，否则是查看其他用户的个人主页
            if (userData != null)
            {
                centerViewModel.centerModel.UserData = userData;
            }
            else
            {
                modifyButton.Visibility = Visibility.Hidden;
                centerViewModel.load_userinfo(uid);
            }
            introduce.Text = centerViewModel.centerModel.UserData.Introduce;
            BitmapImage image = ByteArrayToImage(centerViewModel.centerModel.UserData.Avatar);
            Avatar.img.Source = image;
            //绑定用户头像数据
            TransformGroup transformGroup = Avatar.img.RenderTransform as TransformGroup;
            ScaleTransform sfr = transformGroup.Children[0] as ScaleTransform;
            TranslateTransform tlt = transformGroup.Children[1] as TranslateTransform;
            BindingOperations.SetBinding(sfr, ScaleTransform.ScaleXProperty, new Binding("centerModel.UserData.AvatarLastScaleX"));
            BindingOperations.SetBinding(sfr, ScaleTransform.ScaleYProperty, new Binding("centerModel.UserData.AvatarLastScaleY"));
            BindingOperations.SetBinding(sfr, ScaleTransform.CenterXProperty, new Binding("centerModel.UserData.LastCenterPointX"));
            BindingOperations.SetBinding(sfr, ScaleTransform.CenterYProperty, new Binding("centerModel.UserData.LastCenterPointY"));
            BindingOperations.SetBinding(tlt, TranslateTransform.XProperty, new Binding("centerModel.UserData.LastX"));
            BindingOperations.SetBinding(tlt, TranslateTransform.YProperty, new Binding("centerModel.UserData.LastY"));
        }

        private home _parentWin;
        public home ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
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

        //编辑个人简介
        private void Modify_Introduce(object sender, MouseButtonEventArgs e)
        {
            string modify = "\ue619", save = "\ue936";
            TextBlock textBlock = sender as TextBlock;
            //进行编辑
            if (textBlock.Text == modify)
            {
                textBlock.Text = save;
                introduce.IsReadOnly = false;
                introduce.Background = Brushes.White;
            }
            //编辑完保存
            else if (textBlock.Text == save)
            {
                textBlock.Text = modify;
                introduce.IsReadOnly = true;
                introduce.Background = Brushes.Transparent;
                try
                {
                    centerViewModel.centerModel.UserData.Introduce = introduce.Text;
                    //调用ViewModel的save_introduce函数更新数据库
                    centerViewModel.save_introduce();
                }
                catch (Exception)
                {
                    textBlock.Text = string.Empty;
                }
            }
        }

        //调整编辑框的行数
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int maxLines = 4;
            TextBox textBox = sender as TextBox;
            int currentLines = textBox.LineCount;
            if (currentLines == 4)
                textBox.AcceptsReturn = false;
            else textBox.AcceptsReturn = true;
            if (currentLines > maxLines)
            {
                textBox.Text = textBox.Text.Remove(textBox.GetCharacterIndexFromLineIndex(currentLines - 1));
                textBox.CaretIndex = textBox.Text.Length;
            }
        }

        //查看该用户发布的帖子
        private void Watch_Post(object sender, RoutedEventArgs e)
        {
            ParentWindow.jump_to_userpost(centerViewModel.centerModel.UserData.Uid, centerViewModel.centerModel.UserData.Username);
        }

        //查看该用户收藏/提交的题头
        private void Watch_Head(object sender, RoutedEventArgs e)
        {
            ParentWindow.jump_to_userhead(centerViewModel.centerModel.UserData.Uid, centerViewModel.centerModel.UserData.Username);
        }
    }
}
