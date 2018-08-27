using System.Collections.ObjectModel;

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
