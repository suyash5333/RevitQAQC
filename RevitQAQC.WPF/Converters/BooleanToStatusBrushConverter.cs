using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace RevitQAQC.WPF.Converters
{
    public class BooleanToStatusBrushConverter : IValueConverter
    {
        private static readonly SolidColorBrush PassBrush =
            new SolidColorBrush(Color.FromRgb(0x22, 0xC5, 0x5E)); // green

        private static readonly SolidColorBrush FailBrush =
            new SolidColorBrush(Color.FromRgb(0xDC, 0x26, 0x26)); // red

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isPass)
            {
                if (isPass)
                    PassBrush.Freeze();
                else
                    FailBrush.Freeze();

                return isPass ? PassBrush : FailBrush;
            }

            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}