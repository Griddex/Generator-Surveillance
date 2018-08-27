using System;
using System.Globalization;
using System.Windows.Data;

namespace Panel.Converters
{
    public class ConvertIntToTwoCharString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int v = (int)value;
            return v.ToString("D2");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = (string)value;
            return int.Parse(s);
        }
    }
}
