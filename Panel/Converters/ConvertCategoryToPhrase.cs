using System;
using System.Globalization;
using System.Windows.Data;

namespace Panel.Converters
{
    public class ConvertCategoryToPhrase : IValueConverter
    {
        public object Convert(object value, Type targetType, 
            object parameter, CultureInfo culture)
        {
            return $"Category: {value.ToString()}";
        }

        public object ConvertBack(object value, Type targetType, 
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
