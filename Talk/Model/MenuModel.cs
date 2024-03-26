using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    public class UserData
    {
        public string Uid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
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
    }
    class MenuModel : Common.NotifyBase
    {
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
