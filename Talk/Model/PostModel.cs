using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    //帖子页面的数据模型

    //帖子类
    public class Post
    {
        public string Pid { get; set; }
        public string Section { get; set; }
        public string AuthorId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public int ClickCount { get; set; }
        public DateTime? LastClick { get; set; }
        public DateTime? LastReply { get; set; }
    }

    //回帖类
    public class Reply
    {
        public string Rid { get; set; }
        public string Author { get; set; }
        public string AuthorId { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
    }

    //用户数据类
    public class UserData3
    {
        public string Username { get; set; }
        public byte[] Avatar { get; set; }
        public float AvatarLastScaleX { get; set; }
        public float AvatarLastScaleY { get; set; }
        public float LastCenterPointX { get; set; }
        public float LastCenterPointY { get; set; }
        public float LastX { get; set; }
        public float LastY { get; set; }
    }
    class PostModel : Common.NotifyBase
    {
        public PostModel()
        {
            Replys = new List<Reply>();
        }

        //存放帖子数据
        private Post _postInfo;
        public Post PostInfo
        {
            get { return _postInfo; }
            set
            {
                _postInfo = value;
                DoNotify("PostInfo");
            }
        }

        private UserData3 _authorInfo;
        public UserData3 AuthorInfo
        {
            get { return _authorInfo; }
            set
            {
                _authorInfo = value;
                DoNotify("AuthorInfo");
            }
        }
        private List<Reply> _replys;

        public List<Reply> Replys
        {
            get { return _replys; }
            set
            {
                _replys = value;
                DoNotify("Replys");
            }
        }
        private string _replyText;

        public string ReplyText
        {
            get { return _replyText; }
            set
            {
                _replyText = value;
                DoNotify("ReplyText");
            }
        }
        private string _userId;

        public string UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                DoNotify("UserId");
            }
        }
    }
}
