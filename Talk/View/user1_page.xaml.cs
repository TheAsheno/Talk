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
    //管理端用户管理页面
    public partial class user1_page : Page
    {
        User1ViewModel user1ViewModel;
        public user1_page()
        {
            InitializeComponent();
            user1ViewModel = new User1ViewModel();
            DataContext = user1ViewModel;
        }
        private administer _parentWin;
        public administer ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        //修改用户数据
        private void Modify_User(object sender, RoutedEventArgs e)
        {
            //没有选中数据行则返回
            if (user1ViewModel.user1Model.SelectedUser == null)
                return;
            modify_user modify_User = new modify_user();
            modify_User.DataContext = user1ViewModel;
            UserData2 userData = user1ViewModel.user1Model.SelectedUser;
            //存放修改前的数据
            user1ViewModel.user1Model.ModifyInfo = new UserData2
            {
                Uid = userData.Uid,
                Regdate = userData.Regdate,
                Username = userData.Username,
                Password = userData.Password,
                Sex = userData.Sex,
                Birthday = userData.Birthday,
                Email = userData.Email,
            };
            modify_User.ShowDialog();
            
        }
    }
}
