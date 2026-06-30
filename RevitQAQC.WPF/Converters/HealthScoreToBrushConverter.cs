using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace RevitQAQC.WPF.Converters
{
    public class HealthScoreToBrushConverter : IValueConverter
    {
        private static readonly SolidColorBrush GoodBrush =
            new SolidColorBrush(Color.FromRgb(0x22, 0xC5, 0x5E));  // green  >= 75

        private static readonly SolidColorBrush WarningBrush =
            new SolidColorBrush(Color.FromRgb(0xF5, 0x9E, 0x0B));  // orange 50-74

        private static readonly SolidColorBrush PoorBrush =
            new SolidColorBrush(Color.FromRgb(0xDC, 0x26, 0x26));  // red    < 50

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double score = 0;

            if (value is double d) score = d;
            else if (value is int i) score = i;
            else if (value != null) double.TryParse(value.ToString(), out score);

            SolidColorBrush brush;

            if (score >= 75)
                brush = GoodBrush;
            else if (score >= 50)
                brush = WarningBrush;
            else
                brush = PoorBrush;

            brush.Freeze();
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}