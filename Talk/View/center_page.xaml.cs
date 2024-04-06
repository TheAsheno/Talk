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
    /// <summary>
    /// center_page.xaml 的交互逻辑
    /// </summary>
    public partial class center_page : Page
    {
        CenterViewModel centerViewModel;
        public center_page(string uid, UserData userData = null)
        {
            InitializeComponent();
            centerViewModel = new CenterViewModel();
            DataContext = centerViewModel;
            if (userData != null)
                centerViewModel.centerModel.UserData = userData;
            else
                centerViewModel.load_userinfo(uid);
            introduce.Text = userData.Introduce;
            BitmapImage image = ByteArrayToImage(centerViewModel.centerModel.UserData.Avatar);
            Avatar.img.Source = image;
            
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
        private void Modify_Introduce(object sender, MouseButtonEventArgs e)
        {
            string modify = "\ue619", save = "\ue936";
            TextBlock textBlock = sender as TextBlock;
            if (textBlock.Text == modify)
            {
                textBlock.Text = save;
                introduce.IsReadOnly = false;
                introduce.Background = Brushes.White;
            }
            else if (textBlock.Text == save)
            {
                textBlock.Text = modify;
                introduce.IsReadOnly = true;
                introduce.Background = Brushes.Transparent;
                try
                {
                    centerViewModel.centerModel.UserData.Introduce = introduce.Text;
                    centerViewModel.save_introduce();
                }
                catch (Exception)
                {
                    textBlock.Text = string.Empty;
                }
            }
        }
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
    }
}
