using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Models.TableModels
{
    class GeneratorFuellingTableModel
    {
        public int SN { get; set; }
        public string Event { get; set; }
        public DateTime Date { get; set; }
        public double VolumeOfDiesel { get; set; }
        public double CostOfDiesel { get; set; }
        public string Vendor { get; set; }
    }
}
