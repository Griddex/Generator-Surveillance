using System;

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
