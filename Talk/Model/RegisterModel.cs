using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Talk.Model
{
    //注册页面的数据模型
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

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                DoNotify("Email");
            }
        }

        private string _sex = "\ue611";
        public string Sex
        {
            get { return _sex; }
            set
            {
                _sex = value;
                DoNotify("Sex");
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

        private float _lastX = 0;

        public float LastX
        {
            get { return _lastX; }
            set
            {
                _lastX = value;
                DoNotify("LastX");
            }
        }

        private float _lastY = 0;

        public float LastY
        {
            get { return _lastY; }
            set
            {
                _lastY = value;
                DoNotify("LastY");
            }
        }
    }
}
