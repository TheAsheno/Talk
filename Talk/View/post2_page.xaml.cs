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
using Talk.Model;

namespace Talk.View
{
    //管理端帖子管理页面
    public partial class post2_page : Page
    {
        Post2ViewModel post2ViewModel;
        public post2_page()
        {
            InitializeComponent();
            post2ViewModel = new Post2ViewModel();
            DataContext = post2ViewModel;
            //加载版块记录
            for (int i = 0; i < post2ViewModel.post2Model.Sections.Count(); i++)
            {
                // 创建新的 ComboBoxItem 实例
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = post2ViewModel.post2Model.Sections[i].Name;
                newItem.Tag = post2ViewModel.post2Model.Sections[i].Sid;
                // 将新的 ComboBoxItem 添加到 ComboBox 中
                SelectSection.Items.Add(newItem);
            }
        }
        private administer _parentWin;
        public administer ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        //修改帖子
        private void Modify_Post(object sender, RoutedEventArgs e)
        {
            if (post2ViewModel.post2Model.SelectedPost == null)
                return;
            modify_post modify_Post = new modify_post(post2ViewModel.post2Model.Sections, post2ViewModel.post2Model.SelectedPost.Section);
            modify_Post.DataContext = post2ViewModel;
            Post postData = post2ViewModel.post2Model.SelectedPost;
            //存放修改前的数据
            post2ViewModel.post2Model.ModifyInfo = new Post
            {
                Pid = postData.Pid,
                Section = postData.Section,
                AuthorId = postData.AuthorId,
                Title = postData.Title,
                Time = postData.Time,
                ClickCount = postData.ClickCount,
            };
            modify_Post.ShowDialog();
        }
    }
}
