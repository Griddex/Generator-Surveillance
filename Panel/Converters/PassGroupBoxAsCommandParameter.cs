using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Panel.Converters
{
    class PassGroupBoxAsCommandParameter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new Tuple<GroupBox>((GroupBox)values[0]);
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
