using Panel.Models.InputModels;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Panel.Converters
{
    public class InputViewMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            GeneratorInformationModel generatorInformationModel = new GeneratorInformationModel();
            foreach (var item in values)
            {
                if (item is DatePicker) generatorInformationModel.dteGenInfo = item as DatePicker;
                else if (item is ComboBox) generatorInformationModel.cmbxGenInfo = item as ComboBox;
            }
            return generatorInformationModel;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
