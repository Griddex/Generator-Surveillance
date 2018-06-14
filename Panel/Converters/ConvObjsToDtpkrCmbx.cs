using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Panel.Converters
{
    public class ConvObjsToDtpkrCmbx : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            ArrayList arrayList = new ArrayList();
            foreach (var o in values)
            {
                if (o is DatePicker)
                    arrayList.Add(o as DatePicker);
                else if (o is ComboBox)
                    arrayList.Add(o as ComboBox);
            }
            return arrayList;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
