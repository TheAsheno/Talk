using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    //管理端添加帖子页面的数据模型
    class Post1Model : Common.NotifyBase
    {
        public Post1Model()
        {
            Sections = new List<Section2>();
        }

        //存放版块记录，选择版块时需要用到
        private List<Section2> _sections;

        public List<Section2> Sections
        {
            get { return _sections; }
            set
            {
                _sections = value;
                DoNotify("Sections");
            }
        }
        private string _postSection;
        public string PostSection
        {
            get { return _postSection; }
            set
            {
                _postSection = value;
                DoNotify("PostSection");
            }
        }

        private string _postTitle;
        public string PostTitle
        {
            get { return _postTitle; }
            set
            {
                _postTitle = value;
                DoNotify("PostTitle");
            }
        }

        private string _content;
        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                DoNotify("Content");
            }
        }
    }
}
