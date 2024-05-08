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
using Talk.ViewModel;
using Talk.Model;

namespace Talk.View
{
    //管理端修改帖子窗口
    public partial class modify_post : Window
    {
        public modify_post(List<Section2> Sections, string SelectSectionName)
        {
            InitializeComponent();
            Post2ViewModel post2ViewModel = DataContext as Post2ViewModel;
            //加载版块记录
            for (int i = 0; i < Sections.Count(); i++)
            {
                // 创建新的 ComboBoxItem 实例
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = Sections[i].Name;
                newItem.Tag = Sections[i].Sid;
                // 将新的 ComboBoxItem 添加到 ComboBox 中
                SelectSection.Items.Add(newItem);
            }
        }
        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
