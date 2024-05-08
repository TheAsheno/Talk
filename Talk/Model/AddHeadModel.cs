using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    //添加题头页面的数据模型
    class AddHeadModel : Common.NotifyBase
    {
        private string _headText;
        public string HeadText
        {
            get { return _headText; }
            set
            {
                _headText = value;
                DoNotify("HeadText");
            }
        }

        private string _author = "匿名";
        public string Author
        {
            get { return _author; }
            set
            {
                _author = value;
                DoNotify("Author");
            }
        }
    }
}
