using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Talk.Common
{
    //将进度条的值转换为一个矩形，用于在界面上表示进度条的进度（每次刷新或者进入到新页面时顶部的蓝色滑动条）
    class ProgressBarValueToRectConverter : IValueConverter
    {
        //矩形的高度固定为 4，宽度则根据进度条的值而变化
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double progressBarValue = (double)value;
            double progressBarWidth = double.IsNaN(progressBarValue) ? 0 : progressBarValue;

            return new Rect(0, 0, progressBarWidth, 4);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
