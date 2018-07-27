using Panel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Repositories
{
    public class GeneratorFuellingRepository : Repository<GeneratorFuelling>, 
        IGeneratorFuellingRepository
    {
        public GeneratorFuellingRepository(GeneratorSurveillanceDBEntities context) 
            : base(context) { }

        public GeneratorSurveillanceDBEntities GeneratorSurveillanceDBContext
        {
            get { return Context as GeneratorSurveillanceDBEntities; }
        }

        public void AddFuelPurchaseRecord(DateTime Fuellingdate, 
            string Vendor, double Volumeofdiesel, double Costofdiesel)
        {
            int NoOfRecords = GeneratorSurveillanceDBContext
                                    .GeneratorFuellings
                                    .Count();
            GeneratorSurveillanceDBContext.GeneratorFuellings
                                          .Add
            (
                new GeneratorFuelling
                {
                    Id = NoOfRecords,
                    Date = Fuellingdate,
                    Vendor = Vendor,
                    VolumeOfDiesel = Volumeofdiesel,
                    CostOfDiesel = Costofdiesel
                }
            );
        }

        public void AddFuelConsumptionHours(string GeneratorName, 
            DateTime RunningHoursDate, 
            double RunningHours,
            double CumFuelVolumeSinceLastReading)
        {
            int NoOfRecords = GeneratorSurveillanceDBContext
                                        .GeneratorRunningHrs
                                        .Count();
            GeneratorSurveillanceDBContext.GeneratorRunningHrs.Add
            (
                new GeneratorRunningHr
                {
                    Id = NoOfRecords,
                    Generator = GeneratorName,
                    Date = RunningHoursDate,
                    CumFuelVolumeSinceLastReading = 
                            CumFuelVolumeSinceLastReading,
                    RunningHours = RunningHours
                }
            );
        }

        public ObservableCollection<GeneratorFuelling> GetAllGeneratorFuellings()
        {
            var AllGeneratorFuellings = new ObservableCollection<GeneratorFuelling>
                                        (
                                            GeneratorSurveillanceDBContext
                                            .GeneratorFuellings
                                            .AsParallel<GeneratorFuelling>()
                                        );
            return AllGeneratorFuellings;
        }

        public ObservableCollection<GeneratorFuelling> GetAnyPageGeneratorFuellings(
            int pageIndex = 1, int pageSize = 10)
        {
            int SkipBy = (pageIndex == 1) ? (pageIndex - 1) * pageSize : 
                                        ((pageIndex - 1) * pageSize) - 1;

            var FirstPageGeneratorFuellings = new ObservableCollection<GeneratorFuelling>
                                            (
                                                GeneratorSurveillanceDBContext
                                                .GeneratorFuellings
                                                .OrderBy(x => x.Id)
                                                .Skip(SkipBy)
                                                .Take(pageSize)
                                                .AsParallel<GeneratorFuelling>()
                                            );
            return FirstPageGeneratorFuellings;
        }

        public (double Curr, double Test, double Stnd) GetFuelConsumptionData(
            string GeneratorName)
        {
            var LastRowCompSetting = GeneratorSurveillanceDBContext
                                            .ConsumptionSettings
                                            .Where(x => x.GeneratorName ==
                                                        GeneratorName)
                                            .OrderBy(x => x.Date)
                                            .FirstOrDefault();

            double TestFuelComp = LastRowCompSetting.TestConsumption;
            double StndFuelComp = LastRowCompSetting.StandardConsumption;

            var LastRowRunningHrs = GeneratorSurveillanceDBContext
                                            .GeneratorRunningHrs
                                            .Where(x => x.Generator ==
                                                  GeneratorName)
                                            .OrderBy(x => x.Date)
                                            .FirstOrDefault();

            var PenultimateRowRunningHrs = GeneratorSurveillanceDBContext
                                                .GeneratorRunningHrs
                                                .Where(x => x.Generator ==
                                                      GeneratorName)
                                                .OrderBy(x => x.Date)
                                                .Skip(1)
                                                .FirstOrDefault();

            double CurrFuelComp = (LastRowRunningHrs
                                        .CumFuelVolumeSinceLastReading
                                    - PenultimateRowRunningHrs
                                            .CumFuelVolumeSinceLastReading) /
                            (LastRowRunningHrs
                                    .RunningHours - PenultimateRowRunningHrs
                                                            .RunningHours);

            return (TestFuelComp, StndFuelComp, CurrFuelComp);
        }

    }
}
