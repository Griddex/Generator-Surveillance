using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Panel.Converters
{
    class PassPswbxExpdrExpdrStkPlVwBxAsCommandParameters : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, 
            object parameter, CultureInfo culture)
        {
            return new Tuple<PasswordBox, Expander,
                Expander, StackPanel,
                Viewbox>
                (
                    (PasswordBox)values[0], 
                    (Expander)values[1],
                    (Expander)values[2],
                    (StackPanel)values[3],
                    (Viewbox)values[4]
                );
        }


        public object[] ConvertBack(object value, Type[] targetTypes, 
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
