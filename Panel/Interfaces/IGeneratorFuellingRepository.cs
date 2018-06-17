using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Interfaces
{
    public interface IGeneratorFuellingRepository : IRepository<GeneratorFuelling>
    {
        void AddFuelPurchaseRecord(DateTime Fuellingdate, string Vendor, double Volumeofdiesel, double Costofdiesel);
        void AddFuelConsumptionHours(string GeneratorName, DateTime RunningHoursDate, double RunningHours, double CumFuelVolumeSinceLastReading);        
        ObservableCollection<GeneratorFuelling> GetAnyPageGeneratorFuellings(int pageIndex = 1, int pageSize = 10);
    }
}
