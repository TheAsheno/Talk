using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Model
{
    public class HeadInfo
    {
        public string Text { get; set; }
        public string Author { get; set; }
        public int Like { get; set; }
    }
    class HomeModel : Common.NotifyBase
    {
        public HomeModel()
        {
            HeadInfo = new ObservableCollection<HeadInfo>();
        }
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
    }
}
