using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    //个人中心页面的数据模型
    class CenterModel : Common.NotifyBase
    {
        
        private UserData _userdata;
        public UserData UserData
        {
            get { return _userdata; }
            set
            {
                _userdata = value;
                DoNotify("UserData");
            }
        }
    }
}
