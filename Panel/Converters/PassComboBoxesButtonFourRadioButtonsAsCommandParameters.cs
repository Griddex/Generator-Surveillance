using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using Panel.Models.InputModels;

namespace Panel.Converters
{
    class PassComboBoxesButtonFourRadioButtonsAsCommandParameters : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //var lasttwoRadiobuttons = (Tuple<RadioButton, RadioButton>)values[7];

            //return new Tuple<ComboBox, ComboBox, 
            //                 ComboBox, ComboBox, 
            //                 Button, RadioButton,
            //                 RadioButton, Tuple<RadioButton,RadioButton>>(
            //                                    (ComboBox)values[0], 
            //                                    (ComboBox)values[1],
            //                                    (ComboBox)values[2],
            //                                    (ComboBox)values[3],
            //                                    (Button)values[4],
            //                                    (RadioButton)values[5],
            //                                    (RadioButton)values[6],                                                
            //                                    (Tuple<RadioButton, RadioButton>)values[7]
            //                        );

            var StartedControls = new StartedControls()
            {
                cmbx1 = (ComboBox)values[0],
                cmbx2 = (ComboBox)values[1],
                cmbx3 = (ComboBox)values[2],
                cmbx4 = (ComboBox)values[3],
                btn1 = (Button)values[4],
                rdbtn1 = (RadioButton)values[5],
                rdbtn2 = (RadioButton)values[6],
                rdbtn3 = (RadioButton)values[7],
                rdbtn4 = (RadioButton)values[8],
            };
            return StartedControls;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
