using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Panel.Models.InputModels
{
    public class GeneratorInformationModel : ValidateNotifyModelBase
    {
        public DatePicker dteGenInfo { get; set; }
        public ComboBox cmbxGenInfo { get; set; }
    }
}
