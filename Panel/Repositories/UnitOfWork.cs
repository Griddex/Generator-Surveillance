using Panel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork()
        {

        }
        private readonly GeneratorSurveillanceDBEntities _context;
        public UnitOfWork(GeneratorSurveillanceDBEntities context)
        {
            _context = context;
            GeneratorUsage = new GeneratorUsageRepository(_context);
            GeneratorInformation = new GeneratorInformationRepository(_context);
            GeneratorScheduler = new GeneratorSchedulerRepository(_context);
            GeneratorRunningHr = new GeneratorRunningHrsRepository(_context);
            GeneratorMaintenance = new GeneratorMaintenanceRepository(_context);
            GeneratorFuelling = new GeneratorFuellingRepository(_context);
        }

        public IGeneratorUsageRepository GeneratorUsage { get; private set; }
        public IGeneratorInformationRepository GeneratorInformation { get; private set; }
        public IGeneratorSchedulerRepository GeneratorScheduler { get; private set; }
        public IGeneratorRunningHrsRepository GeneratorRunningHr { get; private set; }
        public IGeneratorMaintenanceRepository GeneratorMaintenance { get; private set; }
        public IGeneratorFuellingRepository GeneratorFuelling { get; private set; }

        public int Complete()
        {
           return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
