using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    //论坛首页页面的数据模型

    //滚动题头栏的数据类
    public class HeadInfo
    {
        public string Text { get; set; }
        public string Author { get; set; }
        public string Hid { get; set; }
    }

    //推荐帖子栏的数据类
    public class Content
    {
        public string Title { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string Pid { get; set; }
    }
    class MainModel : Common.NotifyBase
    {
        public MainModel()
        {
            HeadInfo = new ObservableCollection<HeadInfo>();
            HotContent = new ObservableCollection<Content>();
            NewContent = new ObservableCollection<Content>();
        }

        //存放题头信息
        private ObservableCollection<HeadInfo> _headInfo;
        public ObservableCollection<HeadInfo> HeadInfo
        {
            get { return _headInfo; }
            set
            {
                _headInfo = value;
                DoNotify("HeadInfo");
            }
        }

        private ObservableCollection<Content> _hotcontent;
        public ObservableCollection<Content> HotContent
        {
            get { return _hotcontent; }
            set
            {
                _hotcontent = value;
                DoNotify("HotContent");
            }
        }
        private ObservableCollection<Content> _newcontent;
        public ObservableCollection<Content> NewContent
        {
            get { return _newcontent; }
            set
            {
                _newcontent = value;
                DoNotify("NewContent");
            }
        }
    }
}
