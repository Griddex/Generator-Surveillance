using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Panel.Converters
{
    class PassDtPkrTxBxTwoCmboxsBtnAsCommandParameters : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, 
            CultureInfo culture)
        {
            return new Tuple<PasswordBox, 
                             DatePicker, 
                             TextBox, 
                             ComboBox, 
                             ComboBox, 
                             StackPanel,
                             Expander,
                             Tuple<Button>>(
                                        (PasswordBox)values[0],
                                        (DatePicker)values[1],
                                        (TextBox)values[2], 
                                        (ComboBox)values[3], 
                                        (ComboBox)values[4],
                                        (StackPanel)values[5],
                                        (Expander)values[6],
                                        new Tuple<Button>((Button)values[7])
                                    );
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, 
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
