using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    public class UserData2
    {
        public string Uid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public DateTime Birthday { get; set; }
    }
    class User1Model : Common.NotifyBase
    {
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
    }
}
