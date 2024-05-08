using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    //管理端管理帖子页面的数据模型
    class Post2Model : Common.NotifyBase
    {
        public Post2Model()
        {
            Posts = new List<Post>();
            Sections = new List<Section2>();
        }

        //存放全部帖子记录
        private List<Post> _posts;

        public List<Post> Posts
        {
            get { return _posts; }
            set
            {
                _posts = value;
                DoNotify("Posts");
            }
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

        private string _pid;
        public string Pid
        {
            get { return _pid; }
            set
            {
                _pid = value;
                DoNotify("Pid");
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

        //获取当前页面下对应的若干条记录
        public List<Post> GetList(int startNum, int count)
        {
            return Posts.Skip(startNum).Take(count).Cast<Post>().ToList();
        }
        public int GetTotalCount
        {
            get
            {
                return Posts.Count;
            }
        }

        //绑定datagrid选中的那行数据
        private Post _selectedPost;
        public Post SelectedPost
        {
            get { return _selectedPost; }
            set
            {
                _selectedPost = value;
                DoNotify("SelectedPost");
            }
        }

        //存放修改后的数据
        private Post _modifyInfo;
        public Post ModifyInfo
        {
            get { return _modifyInfo; }
            set
            {
                _modifyInfo = value;
                DoNotify("ModifyInfo");
            }
        }
    }
}
