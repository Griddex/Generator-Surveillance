using System;

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
