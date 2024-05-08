using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    //版块列表页面的数据模型

    //版块类
    public class Section
    {
        public string Sid { get; set; }
        public string Name { get; set; }
        public string MasterId { get; set; }
        public string MasterName { get; set; }
        public string Statement { get; set; }
        public int ClickCount { get; set; }
        public int PostCount { get; set; }
    }
    class SectionModel : Common.NotifyBase
    {
        public SectionModel()
        {
            Sections = new List<Section>();
        }

        //存放所有版块数据
        private List<Section> _sections;

        public List<Section> Sections
        {
            get { return _sections; }
            set
            {
                _sections = value;
                DoNotify("Sections");
            }
        }
    }
}
