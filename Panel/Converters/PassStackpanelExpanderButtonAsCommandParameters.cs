using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Panel.Converters
{
    class PassStackpanelExpanderButtonAsCommandParameters : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, 
            object parameter, CultureInfo culture)
        {
            return new Tuple<StackPanel, Expander, Button>(
                (StackPanel)values[0], 
                (Expander)values[1],
                (Button)values[2]);
        }


        public object[] ConvertBack(object value, Type[] targetTypes, 
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
