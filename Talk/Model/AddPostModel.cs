using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    //发布帖子页面的数据模型
    public class Section2
    {
        public string Sid { get; set; }
        public string Name { get; set; }
    }
    class AddPostModel : Common.NotifyBase
    {
        public AddPostModel()
        {
            Sections = new List<Section2>();
        }
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

        private string _postContent;

        public string PostContent
        {
            get { return _postContent; }
            set
            {
                _postContent = value;
                DoNotify("PostTitile");
            }
        }
    }
}
