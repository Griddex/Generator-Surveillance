using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
