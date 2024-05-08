using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    //某版块下帖子列表页面的数据模型
    class PostList 
    {
        public string Pid { get; set; }
        public string Title { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
    }

    class PostlistModel : Common.NotifyBase
    {
        public PostlistModel()
        {
            PostList = new List<PostList>();
        }

        //存放某版块下所有帖子的记录
        private List<PostList> _postList;

        public List<PostList> PostList
        {
            get { return _postList; }
            set
            {
                _postList = value;
                DoNotify("Replys");
            }
        }
        private string _sectionName;

        public string SectionName
        {
            get { return _sectionName; }
            set
            {
                _sectionName = value;
                DoNotify("SectionName");
            }
        }
    }
}
