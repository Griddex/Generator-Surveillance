using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Panel.Converters
{
    public class PassDatePickerComboBoxAsCommandParameter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new Tuple<DatePicker, ComboBox>((DatePicker)values[0], (ComboBox)values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
