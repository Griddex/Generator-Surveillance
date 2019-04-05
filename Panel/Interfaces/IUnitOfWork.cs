using System;
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
        Task<int> CompleteAsync();
    }
}
