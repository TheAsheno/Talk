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
    class ProgressBarValueToRectConverter : IValueConverter
    {
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
