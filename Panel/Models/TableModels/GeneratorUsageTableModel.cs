using System;

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
