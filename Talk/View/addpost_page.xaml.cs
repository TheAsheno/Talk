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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Talk.ViewModel;

namespace Talk.View
{
    //发布帖子页面
    public partial class addpost_page : Page
    {
        AddPostViewModel addPostViewModel;
        public addpost_page(string uid)
        {
            InitializeComponent();
            addPostViewModel = new AddPostViewModel(uid);
            DataContext = addPostViewModel;
            //动态设置版块下拉框的子元素
            for (int i = 0; i < addPostViewModel.addPostModel.Sections.Count(); i++)
            {
                // 创建新的 ComboBoxItem 实例
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = addPostViewModel.addPostModel.Sections[i].Name;
                newItem.Tag = addPostViewModel.addPostModel.Sections[i].Sid;
                // 将新的 ComboBoxItem 添加到 ComboBox 中
                SelectSection.Items.Add(newItem);
            }
        }
        private home _parentWin;
        public home ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        //发布帖子
        private void Add_Post(object sender, RoutedEventArgs e)
        {
            if (addPostViewModel.DoAdd())
            {
                //如果发布成功则跳转到该版块的帖子列表页面
                string targetSid = addPostViewModel.addPostModel.PostSection;
                ParentWindow.jump_to_postlist(targetSid, addPostViewModel.addPostModel.Sections
                    .FirstOrDefault(section => section.Sid == targetSid)?.Name);
            }
        }
    }
}
