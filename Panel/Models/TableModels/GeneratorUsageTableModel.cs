using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Models.TableModels
{
    class GeneratorUsageTableModel
    {
        public int SN { get; set; }
        public DateTime Date { get; set; }
        public string GeneratorName { get; set; }
        public DateTime GeneratorStartedOn { get; set; }
        public DateTime GeneratorStoppedOn { get; set; }
    }
}
