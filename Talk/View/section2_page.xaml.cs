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
    //管理端版块管理页面
    public partial class section2_page : Page
    {
        Section2ViewModel section2ViewModel;
        public section2_page()
        {
            InitializeComponent();
            section2ViewModel = new Section2ViewModel();
            DataContext = section2ViewModel;
        }
        private administer _parentWin;
        public administer ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        //修改版块
        private void Modify_Section(object sender, RoutedEventArgs e)
        {
            if (section2ViewModel.section2Model.SelectedSection == null)
                return;
            modify_section modify_Section = new modify_section();
            modify_Section.DataContext = section2ViewModel;
            Model.Section sectionData = section2ViewModel.section2Model.SelectedSection;
            //存放修改前的数据
            section2ViewModel.section2Model.ModifyInfo = new Model.Section
            {
                Sid = sectionData.Sid,
                Name = sectionData.Name,
                MasterId = sectionData.MasterId,
                MasterName = sectionData.MasterName,
                Statement = sectionData.Statement,
                ClickCount = sectionData.ClickCount,
                PostCount = sectionData.PostCount,
            };
            modify_Section.ShowDialog();
        }
    }
}
