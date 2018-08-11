using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Panel.Converters
{
    class PassComboBoxesButtonLabelAsCommandParameters : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new Tuple<ComboBox, 
                             ComboBox, 
                             ComboBox, 
                             ComboBox, 
                             Button,
                             Label>(
                                        (ComboBox)values[0], 
                                        (ComboBox)values[1],
                                        (ComboBox)values[2],
                                        (ComboBox)values[3],
                                        (Button)values[4],
                                        (Label)values[5]
                                    );
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
