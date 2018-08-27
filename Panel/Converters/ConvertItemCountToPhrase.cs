using System;
using System.Globalization;
using System.Windows.Data;

namespace Panel.Converters
{
    public class ConvertItemCountToPhrase : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"       ItemsCount: {value.ToString()}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
