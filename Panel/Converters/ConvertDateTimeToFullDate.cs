using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Panel.Converters
{
    public class ConvertDateTimeToFullDate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strDate = "";
            try
            {
                strDate = value.ToString();
            }
            catch (Exception) { }           
            
            if (!string.IsNullOrEmpty(strDate))
            {
                try
                {
                    string sysDateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                    string genstrDateFormat = sysDateFormat + " hh:mm:ss tt";
                    DateTime genStartedDate = DateTime.ParseExact(strDate, genstrDateFormat, CultureInfo.InvariantCulture);
                    return genStartedDate;
                }
                catch (Exception)
                {
                    string sysDateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                    DateTime genStartedDate = DateTime.ParseExact(strDate, sysDateFormat, CultureInfo.InvariantCulture);
                    return genStartedDate;
                }
                
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
