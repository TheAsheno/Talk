using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    //用户发布帖子页面的数据模型
    class UserPostModel : Common.NotifyBase
    {
        public UserPostModel()
        {
            UserPostList = new List<SearchPost>();
        }

        //存放某用户所有发布的帖子的记录
        private List<SearchPost> _userPostList;

        public List<SearchPost> UserPostList
        {
            get { return _userPostList; }
            set
            {
                _userPostList = value;
                DoNotify("UserPostList");
            }
        }
    }
}
