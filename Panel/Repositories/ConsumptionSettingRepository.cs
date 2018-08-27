using Panel.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Panel.Repositories
{
    public class ConsumptionSettingRepository : Repository<ConsumptionSetting>,
        IConsumptionSettingsRepository
    {
        public ConsumptionSettingRepository(GeneratorSurveillanceDBEntities context)
            : base(context) { }

        public GeneratorSurveillanceDBEntities GeneratorSurveillanceDBContext
        {
            get { return Context as GeneratorSurveillanceDBEntities; }
        }

        public void SetComsumption(DateTime ConsumptionDate, string GeneratorName,
            double CurrentFuelConsumption,
            double TestFuelConsumption, double StandardFuelConsumption)
        {
            int RecordNo = GeneratorSurveillanceDBContext
                                .ConsumptionSettings
                                .Count();
            GeneratorSurveillanceDBContext.ConsumptionSettings.Add
            (
                new ConsumptionSetting
                {
                    Id = RecordNo,
                    Date = ConsumptionDate,
                    GeneratorName = GeneratorName,
                    CurrentConsumption = CurrentFuelConsumption,
                    TestConsumption = TestFuelConsumption,
                    StandardConsumption = StandardFuelConsumption
                }
            );
        }

        public ObservableCollection<ConsumptionSetting> GetAnyConsumptionPage(
            int pageIndex = 1, int pageSize = 10)
        {
            int SkipBy = (pageIndex == 1) ? (pageIndex - 1) * pageSize 
                                          : ((pageIndex - 1) * pageSize) - 1;
            var AnyConsumptionPage = new ObservableCollection<ConsumptionSetting>
                                            (
                                                GeneratorSurveillanceDBContext
                                                .ConsumptionSettings
                                                .OrderBy(x => x.Id)
                                                .Skip(SkipBy)
                                                .Take(pageSize)                                                
                                            );
            return AnyConsumptionPage;
        }
    }
}
