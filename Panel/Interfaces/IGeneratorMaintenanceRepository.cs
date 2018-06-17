using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Interfaces
{
    public interface IGeneratorMaintenanceRepository : IRepository<GeneratorMaintenance>
    {
        void AddUnschMaintenance(string MaintenanceType, DateTime UnschMaintenancedate, string UnschComments, double UnschTotalCost);
        void AddSchMaintenance(string MaintenanceType, DateTime SchMaintenancedate, string SchComments, double SchTotalCost);        
        ObservableCollection<GeneratorMaintenance> GetAnyPageGeneratorMaintenance(int pageIndex = 1, int pageSize = 10);
    }
}
