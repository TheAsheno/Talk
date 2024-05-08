using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    //用户提交/收藏题头页面的数据模型
    class HeadData2
    {
        public string Text { get; set; }
        public string Examine { get; set; }
    }
    class HeadData3
    {
        public string Text { get; set; }
        public string Author { get; set; }
    }
    class UserHeadModel : Common.NotifyBase
    {
        public UserHeadModel()
        {
            UserHeadSubmitList = new List<HeadData2>();
            UserHeadCollectList = new List<HeadData3>();
        }

        //存放某用户所有提交的题头的记录
        private List<HeadData2> _userHeadSubmitList;

        public List<HeadData2> UserHeadSubmitList
        {
            get { return _userHeadSubmitList; }
            set
            {
                _userHeadSubmitList = value;
                DoNotify("UserHeadSubmitList");
            }
        }

        //存放某用户所有收藏的题头的记录
        private List<HeadData3> _userHeadCollectList;

        public List<HeadData3> UserHeadCollectList
        {
            get { return _userHeadCollectList; }
            set
            {
                _userHeadCollectList = value;
                DoNotify("UserHeadCollectList");
            }
        }
    }
}
