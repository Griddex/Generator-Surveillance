using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Models.TableModels
{
    class GeneratorMaintenancetableModel
    {
        public int SN { get; set; }
        public string Event { get; set; }
        public DateTime Date { get; set; }
        public double TotalCost { get; set; }
        public string Comments { get; set; }
    }
}
