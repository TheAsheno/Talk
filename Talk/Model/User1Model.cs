using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    //管理端用户管理页面的数据模型
    public class UserData2
    {
        public string Uid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public string Birthday { get; set; }
        public DateTime Regdate { get; set; }
    }
    class User1Model : Common.NotifyBase
    {
        public User1Model()
        {
            Users = new List<UserData2>();
        }
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                DoNotify("UserName");
            }
        }

        private string _uid;
        public string Uid
        {
            get { return _uid; }
            set
            {
                _uid = value;
                DoNotify("Uid");
            }
        }

        private string _sex;
        public string Sex
        {
            get { return _sex; }
            set
            {
                _sex = value;
                DoNotify("Sex");
            }
        }

        //存放全部用户记录
        private List<UserData2> _users;

        public List<UserData2> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                DoNotify("Users");
            }
        }

        //获取当前页面下对应的若干条记录
        public List<UserData2> GetList(int startNum, int count)
        {
            return Users.Skip(startNum).Take(count).Cast<UserData2>().ToList();
        }
        public int GetTotalCount
        {
            get
            {
                return Users.Count;
            }
        }

        //绑定datagrid选中的那行数据
        private UserData2 _selectedUser;
        public UserData2 SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                DoNotify("SelectedUser");
            }
        }

        //存放修改后的数据
        private UserData2 _modifyInfo;
        public UserData2 ModifyInfo
        {
            get { return _modifyInfo; }
            set
            {
                _modifyInfo = value;
                DoNotify("ModifyInfo");
            }
        }
    }
}
