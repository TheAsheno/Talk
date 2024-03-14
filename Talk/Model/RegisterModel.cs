using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    class RegisterModel : Common.NotifyBase
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

        private string _passWord2;

        public string PassWord2
        {
            get { return _passWord2; }
            set
            {
                _passWord2 = value;
                this.DoNotify("Password2");
            }
        }

        private string _birthday = "Birthday";

        public string Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                this.DoNotify("Birthday");
            }
        }
    }
}
