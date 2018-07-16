using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Panel.Converters
{
    class PassComboBoxesAndButtonAsCommandParameters : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new Tuple<ComboBox, ComboBox, ComboBox, ComboBox, Button>(
                (ComboBox)values[0], 
                (ComboBox)values[1],
                (ComboBox)values[2],
                (ComboBox)values[3],
                (Button)values[4]
                );
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
