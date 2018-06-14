using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGeneratorUsageRepository GeneratorUsage { get; }
        IGeneratorSchedulerRepository GeneratorScheduler { get; }
        IGeneratorRunningHrsRepository GeneratorRunningHr { get; }
        IGeneratorMaintenanceRepository GeneratorMaintenance { get; }
        IGeneratorFuellingRepository GeneratorFuelling { get; }
        int Complete();
    }
}
