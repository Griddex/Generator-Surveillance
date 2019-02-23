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

        public void SetConsumption(DateTime ConsumptionDate, string GeneratorName,
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

        public Tuple<double,double> GetTestStandardConsumption(string GeneratorName)
        {
            var LastRowCompSetting = GeneratorSurveillanceDBContext
                                            .ConsumptionSettings
                                            .Where(x => x.GeneratorName ==
                                                        GeneratorName)
                                            .OrderByDescending(x => x.Date)
                                            .FirstOrDefault();
            return new Tuple<double, double>(LastRowCompSetting.TestConsumption,
                                            LastRowCompSetting.StandardConsumption);
        }

        public ObservableCollection<ConsumptionSetting> GetAnyConsumptionPage()
        {
            return new ObservableCollection<ConsumptionSetting>
            (
                GeneratorSurveillanceDBContext
                .ConsumptionSettings
                .AsParallel()
            );
        }
    }
}
