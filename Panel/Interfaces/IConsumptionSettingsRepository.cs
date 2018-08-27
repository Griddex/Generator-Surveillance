using System;
using System.Collections.ObjectModel;

namespace Panel.Interfaces
{
    public interface IConsumptionSettingsRepository
    {
        void SetComsumption(DateTime ConsumptionDate, string GeneratorName, 
            double CurrentFuelConsumption,
            double TestFuelConsumption, double StandardFuelConsumption);

        ObservableCollection<ConsumptionSetting> GetAnyConsumptionPage(
            int pageIndex = 1, int pageSize = 10);
    }
}
