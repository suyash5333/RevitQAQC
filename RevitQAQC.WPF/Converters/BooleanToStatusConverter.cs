using System;
using System.Globalization;
using System.Windows.Data;

namespace RevitQAQC.WPF.Converters
{
    public class BooleanToStatusConverter : IValueConverter
    {
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            if (value is bool isPass)
            {
                return isPass ? "🟢 PASS" : "🔴 FAIL";
            }

            return "Unknown";
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}