using Panel.Models.InputModels;
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
    public class ConvertComboBoxItemToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                return (string)value;
            }
            return null;         
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value is GeneratorNameModel)
                {
                    GeneratorNameModel generatorNameModel = (GeneratorNameModel)value;
                    return generatorNameModel.GeneratorName;
                }                    
                return (string)value;
            }
            return null;
        }
    }
}
