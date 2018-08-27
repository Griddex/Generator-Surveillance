using System;
using System.Globalization;
using System.Windows.Data;

namespace Panel.Converters
{
    public class ConvertYesNoToBoolean : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string YesNo = value.ToString();
            if (YesNo == "Yes")
                return false;
            else
                return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
