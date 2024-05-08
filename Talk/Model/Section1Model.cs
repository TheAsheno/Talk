using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    //管理端添加版块页面的数据类
    class Section1Model : Common.NotifyBase
    {
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

        private string _masterId;
        public string MasterId
        {
            get { return _masterId; }
            set
            {
                _masterId = value;
                DoNotify("MasterId");
            }
        }

        private string _statement;
        public string Statement
        {
            get { return _statement; }
            set
            {
                _statement = value;
                DoNotify("Statement");
            }
        }
    }
}
