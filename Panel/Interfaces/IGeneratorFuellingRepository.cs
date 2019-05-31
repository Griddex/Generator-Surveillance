using System;
using System.Collections.ObjectModel;

namespace Panel.Interfaces
{
    public interface IGeneratorFuellingRepository : IRepository<GeneratorFuelling>
    {
        void AddFuelPurchaseRecord(DateTime Fuellingdate, 
            string Vendor, 
            double Volumeofdiesel, 
            double Costofdiesel);

        bool CheckRunningHoursValidity(double GenHrsInput, string GeneratorName);

        void AddFuelConsumptionHours(string GeneratorName, 
            DateTime RunningHoursDate, 
            double RunningHours, 
            double CumFuelVolumeSinceLastReading);   
        
        ObservableCollection<GeneratorFuelling> GetAnyPageGeneratorFuellings
            (int pageIndex = 1, 
            int pageSize = 10);

        ObservableCollection<GeneratorFuelling> GetAllGeneratorFuellings();

        (double CumFuel, double RunHrs, double Curr,
            double Test, double Stnd) GetFuelConsumptionData(
            string GeneratorName);
    }
}
