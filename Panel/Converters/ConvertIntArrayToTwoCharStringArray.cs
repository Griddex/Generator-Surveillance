using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Panel.Converters
{
    public class ConvertIntArrayToTwoCharStringArray : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<string> valueStringArray = new List<string>();
            foreach (var x in value as IEnumerable<int>)
            {
                valueStringArray.Add(x.ToString("D2"));
            }
            return valueStringArray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
