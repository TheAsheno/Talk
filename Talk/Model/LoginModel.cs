using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    class LoginModel : Common.NotifyBase
    {
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                this.DoNotify("UserName");
            }
        }

        private string _passWord;

        public string PassWord
        {
            get { return _passWord; }
            set
            {
                _passWord = value;
                this.DoNotify("Password");
            }
        }
    }
}
