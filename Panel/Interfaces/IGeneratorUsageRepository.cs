using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Interfaces
{
    public interface IGeneratorUsageRepository : IRepository<GeneratorUsage>
    {
        
        void GeneratorStarted(DateTime RecordDate, string GeneratorName, DateTime GeneratorStarted);
        void GeneratorStopped(DateTime GeneratorStopped);
        ObservableCollection<GeneratorUsage> GetAllGeneratorUsages();
        DateTime GetStartedDate(string GeneratorName);
        DateTime GetStoppedDate(string GeneratorName);
        DateTime GetStartedTime(string GeneratorName);
        DateTime GetStoppedTime(string GeneratorName);
    }
}
