using System;
using System.Collections.ObjectModel;

namespace Panel.Interfaces
{
    public interface IGeneratorMaintenanceRepository : IRepository<GeneratorMaintenance>
    {
        void AddUnschMaintenance(string MaintenanceType, DateTime UnschMaintenancedate, string UnschComments, double UnschTotalCost);
        void AddSchMaintenance(string MaintenanceType, DateTime SchMaintenancedate, string SchComments, double SchTotalCost, string GeneratorName);        
        ObservableCollection<GeneratorMaintenance> GetAnyPageGeneratorMaintenance(int pageIndex = 1, int pageSize = 10);
        ObservableCollection<GeneratorMaintenance> GetAllGeneratorMaintenances();
    }
}
