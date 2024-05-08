using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    //论坛顶部菜单栏的数据模型

    //用户数据类
    public class UserData
    {
        public string Uid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public string Introduce { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime Regdate { get; set; }
        public DateTime? Lastcheck { get; set; }
        public DateTime Lastlog { get; set; }
        public int Checkdays { get; set; }
        public byte[] Avatar { get; set; }
        public float AvatarLastScaleX { get; set; }
        public float AvatarLastScaleY { get; set; }
        public float LastCenterPointX { get; set; }
        public float LastCenterPointY { get; set; }
        public float LastX { get; set; }
        public float LastY { get; set; }
    }
    class MenuModel : Common.NotifyBase
    {
        //存放搜索栏输入文本
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                DoNotify("SearchText");
            }
        }

        //存放用户数据
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
