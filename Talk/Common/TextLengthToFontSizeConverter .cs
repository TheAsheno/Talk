using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Talk.Common
{
    class TextLengthToFontSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                // 根据文本长度计算字体大小
                double baseFontSize = 17; // 初始字体大小
                double scaleFactor = 0.2; // 调整因子

                return baseFontSize - text.Length * scaleFactor;
            }

            return 17; // 默认字体大小
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
