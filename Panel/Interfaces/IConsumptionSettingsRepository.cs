using System;
using System.Collections.ObjectModel;

namespace Panel.Interfaces
{
    public interface IConsumptionSettingsRepository
    {
        void SetConsumption(DateTime ConsumptionDate, string GeneratorName, 
            double CurrentFuelConsumption,
            double TestFuelConsumption, double StandardFuelConsumption);

        Tuple<double, double> GetTestStandardConsumption(string GeneratorName);

        ObservableCollection<ConsumptionSetting> GetAnyConsumptionPage();
    }
}
