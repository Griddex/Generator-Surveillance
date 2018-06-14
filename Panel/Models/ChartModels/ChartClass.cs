using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Models.ChartModels
{
    public class ChartClass : ObservableCollection<string>
    {
        public ChartClass()
        {
            Add("Generator Usage Class");
            Add("Maintenance Charts");
        }
    }
}
