using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Panel.Converters
{
    class PassComboBoxesButtonRadioButtonsAsCommandParameters : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new Tuple<ComboBox, 
                             ComboBox, 
                             ComboBox, 
                             ComboBox, 
                             Button,
                             RadioButton,
                             RadioButton>(
                                        (ComboBox)values[0], 
                                        (ComboBox)values[1],
                                        (ComboBox)values[2],
                                        (ComboBox)values[3],
                                        (Button)values[4],
                                        (RadioButton)values[5],
                                        (RadioButton)values[6]      
                                    );
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
