using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    //管理端题头管理页面的数据模型

    //题头类
    public class HeadData
    {
        public string Hid { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public string Anonymous { get; set; }
        public string Examine { get; set; }
        public DateTime Submit_time { get; set; }
        public DateTime? Audit_time { get; set; }
    }
    class Head2Model : Common.NotifyBase
    {
        private string _hid;
        public string Hid
        {
            get { return _hid; }
            set
            {
                _hid = value;
                DoNotify("Hid");
            }
        }

        private string _author;
        public string Author
        {
            get { return _author; }
            set
            {
                _author = value;
                DoNotify("Author");
            }
        }

        private string _examine;
        public string Examine
        {
            get { return _examine; }
            set
            {
                _examine = value;
                DoNotify("Examine");
            }
        }

        //存放全部题头记录
        private List<HeadData> _heads;

        public List<HeadData> Heads
        {
            get { return _heads; }
            set
            {
                _heads = value;
                DoNotify("Heads");
            }
        }

        //获取当前页面下对应的若干条记录
        public List<HeadData> GetList(int startNum, int count)
        {
            return Heads.Skip(startNum).Take(count).Cast<HeadData>().ToList();
        }
        public int GetTotalCount
        {
            get
            {
                return Heads.Count;
            }
        }

        //绑定datagrid选中的那行数据
        private HeadData _selectedHead;
        public HeadData SelectedHead
        {
            get { return _selectedHead; }
            set
            {
                _selectedHead = value;
                DoNotify("SelectedHead");
            }
        }
    }
}
