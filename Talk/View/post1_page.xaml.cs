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
    //管理端添加帖子页面
    public partial class post1_page : Page
    {
        Post1ViewModel post1ViewModel;
        public post1_page()
        {
            InitializeComponent();
            post1ViewModel = new Post1ViewModel();
            DataContext = post1ViewModel;
            //加载版块记录
            for (int i = 0; i < post1ViewModel.post1Model.Sections.Count(); i++)
            {
                // 创建新的 ComboBoxItem 实例
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = post1ViewModel.post1Model.Sections[i].Name;
                newItem.Tag = post1ViewModel.post1Model.Sections[i].Sid;
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
    }
}
