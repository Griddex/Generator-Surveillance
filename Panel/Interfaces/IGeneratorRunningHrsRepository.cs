using System.Collections.ObjectModel;

namespace Panel.Interfaces
{
    public interface IGeneratorRunningHrsRepository : IRepository<GeneratorRunningHr>
    {
        ObservableCollection<GeneratorRunningHr> GetAllRunningHours();

        ObservableCollection<GeneratorRunningHr> GetAnyPageGeneratorRunningHrs(
            int pageIndex = 1, int pageSize = 10);
    }
}
