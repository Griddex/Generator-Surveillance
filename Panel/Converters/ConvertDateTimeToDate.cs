using System;
using System.Globalization;
using System.Windows.Data;

namespace Panel.Converters
{
    public class ConvertDateTimeToDate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                DateTime dateTime = (DateTime)value;
                return dateTime.ToShortDateString();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
