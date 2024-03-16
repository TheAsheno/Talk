using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
                DoNotify("Password");
            }
        }

        private string _passWord2;

        public string PassWord2
        {
            get { return _passWord2; }
            set
            {
                _passWord2 = value;
                DoNotify("Password2");
            }
        }

        private string _birthday = "Birthday";

        public string Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                DoNotify("Birthday");
            }
        }

        private ImageSource _imageSource = new BitmapImage(new Uri("pack://application:,,,/Asset/images/default.png"));

        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                DoNotify("ImageSource");
            }
        }

        private float _avatarLastScaleX = 1;

        public float AvatarLastScaleX
        {
            get { return _avatarLastScaleX; }
            set
            {
                _avatarLastScaleX = value;
                DoNotify("AvatarLastScaleX");
            }
        }

        private float _avatarLastScaleY = 1;

        public float AvatarLastScaleY
        {
            get { return _avatarLastScaleY; }
            set
            {
                _avatarLastScaleY = value;
                DoNotify("AvatarLastScaleY");
            }
        }

        private float _lastCenterPointX = 0;

        public float LastCenterPointX
        {
            get { return _lastCenterPointX; }
            set
            {
                _lastCenterPointX = value;
                DoNotify("LastCenterPointX");
            }
        }

        private float _lastCenterPointY = 0;

        public float LastCenterPointY
        {
            get { return _lastCenterPointY; }
            set
            {
                _lastCenterPointY = value;
                DoNotify("LastCenterPointY");
            }
        }
    }
}
