using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    //论坛搜索框的数据模型

    //帖子搜索结果类
    class SearchPost
    {
        public string Pid { get; set; }
        public string Title { get; set; }
    }

    //作者搜索结果类
    class SearchAuthor
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
    class SearchModel : Common.NotifyBase
    {
        public SearchModel()
        {
            SearchPostList = new List<SearchPost>();
            SearchUserList = new List<SearchAuthor>();
        }

        //存放帖子搜索结果记录
        private List<SearchPost> _searchPostList;

        public List<SearchPost> SearchPostList
        {
            get { return _searchPostList; }
            set
            {
                _searchPostList = value;
                DoNotify("SearchPostList");
            }
        }

        //存放用户搜索结果记录
        private List<SearchAuthor> _searchUserList;

        public List<SearchAuthor> SearchUserList
        {
            get { return _searchUserList; }
            set
            {
                _searchUserList = value;
                DoNotify("SearchUserList");
            }
        }
    }
}
