using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    //管理端版块管理页面的数据模型
    class Section2Model : Common.NotifyBase
    {
        public Section2Model()
        {
            Sections = new List<Section>();
        }
        private List<Section> _sections;

        //存放全部版块记录
        public List<Section> Sections
        {
            get { return _sections; }
            set
            {
                _sections = value;
                DoNotify("Sections");
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

        private string _sid;
        public string Sid
        {
            get { return _sid; }
            set
            {
                _sid = value;
                DoNotify("Sid");
            }
        }

        //获取当前页面下对应的若干条记录
        public List<Section> GetList(int startNum, int count)
        {
            return Sections.Skip(startNum).Take(count).Cast<Section>().ToList();
        }
        public int GetTotalCount
        {
            get
            {
                return Sections.Count;
            }
        }

        //绑定datagrid选中的那行数据
        private Section _selectedSection;
        public Section SelectedSection
        {
            get { return _selectedSection; }
            set
            {
                _selectedSection = value;
                DoNotify("SelectedSection");
            }
        }

        //存放修改后的数据
        private Section _modifyInfo;
        public Section ModifyInfo
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
