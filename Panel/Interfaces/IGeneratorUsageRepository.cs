using System;
using System.Collections.ObjectModel;

namespace Panel.Interfaces
{
    public interface IGeneratorUsageRepository : IRepository<GeneratorUsage>
    {
        
        void GeneratorStarted(DateTime RecordDate, string GeneratorName, DateTime GeneratorStarted);
        void GeneratorStopped(DateTime GeneratorStopped, int ActiveGenID);
        ObservableCollection<GeneratorUsage> GetAllGeneratorUsages();
        ObservableCollection<GeneratorUsage> GetAnyPageGeneratorUsages(int pageIndex = 1, int pageSize = 10);
        DateTime GetStartedDate(string GeneratorName);
        DateTime GetStoppedDate(string GeneratorName);
        DateTime GetStartedTime(string GeneratorName);
        DateTime GetStoppedTime(string GeneratorName);
    }
}
