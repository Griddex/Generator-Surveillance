using System.Collections.ObjectModel;

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
