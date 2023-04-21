using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Bookstore.Converters
{
    internal class NumericFieldBorderBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var field = (string)value;
            if (!int.TryParse(field, out _)) return Brushes.Red;
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new();
        }
    }
}
