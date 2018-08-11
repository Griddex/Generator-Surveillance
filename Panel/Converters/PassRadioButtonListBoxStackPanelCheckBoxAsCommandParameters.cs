using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Panel.Converters
{
    class PassRadioButtonListBoxStackPanelCheckBoxAsCommandParameters : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new Tuple<TextBlock, 
                             ListBox, 
                             StackPanel,
                             CheckBox>((TextBlock)values[0], 
                                        (ListBox)values[1], 
                                        (StackPanel)values[2],
                                        (CheckBox)values[3]);
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
