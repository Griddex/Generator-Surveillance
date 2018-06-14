using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Models.ChartModels
{
    public class ChartType : ObservableCollection<string>
    {
        public ChartType()
        {
            Add("Line");
            Add("Bar");
            Add("Pie");
            Add("Composite Bar");
        }
    }
}
