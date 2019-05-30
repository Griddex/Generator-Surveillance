using Panel.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Panel.Repositories
{
    public class GeneratorFuellingRepository : Repository<GeneratorFuelling>, IGeneratorFuellingRepository
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
            int RecordNo = GeneratorSurveillanceDBContext.GeneratorFuellings
                .OrderByDescending(x => x.Id).Select(x => x.Id).ToArray()[0];     

            int SNNo = GeneratorSurveillanceDBContext.GeneratorFuellings
                .OrderByDescending(x => x.SN).Select(x => x.SN).ToArray()[0];

            GeneratorSurveillanceDBContext.GeneratorFuellings.Add
            (
                new GeneratorFuelling
                {
                    Id = RecordNo + 1,
                    SN = SNNo + 1,
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
            int RecordNo = GeneratorSurveillanceDBContext.GeneratorRunningHrs
                 .OrderByDescending(x => x.Id).Select(x => x.Id).ToArray()[0];

            int SNNo = GeneratorSurveillanceDBContext.GeneratorRunningHrs
                .OrderByDescending(x => x.SN).Select(x => x.SN).ToArray()[0];

            GeneratorSurveillanceDBContext.GeneratorRunningHrs.Add
            (
                new GeneratorRunningHr
                {
                    Id = RecordNo + 1,
                    SN = SNNo + 1,
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
                                                .OrderBy(x => x.SN)
                                                .Skip(SkipBy)
                                                .Take(pageSize)
                                                .AsParallel<GeneratorFuelling>()
                                            );
            return FirstPageGeneratorFuellings;
        }

        public (double CumFuel, double RunHrs, double Curr, double Test, double Stnd) 
            GetFuelConsumptionData(string GeneratorName)
        {
            double TestFuelComp;
            double StndFuelComp;
            double CurrFuelComp;
            double CumFuelVol;
            double GenRunningHrs;

            var LastRowCompSetting = GeneratorSurveillanceDBContext.ConsumptionSettings
                .Where(x => x.GeneratorName == GeneratorName).OrderByDescending(x => x.Date)
                .FirstOrDefault();

            if(LastRowCompSetting != null)
            {
                TestFuelComp = LastRowCompSetting.TestConsumption;
                StndFuelComp = LastRowCompSetting.StandardConsumption;
            }
            else
            {
                TestFuelComp = 0;
                StndFuelComp = 0;
            }

            var LastRowRunningHrs = GeneratorSurveillanceDBContext
                .GeneratorRunningHrs.Where(x => x.Generator == GeneratorName)
                .OrderByDescending(x => x.Date).ThenByDescending(x => x.CumFuelVolumeSinceLastReading)
                .FirstOrDefault();

            var PenultimateRowRunningHrs = GeneratorSurveillanceDBContext
                .GeneratorRunningHrs.Where(x => x.Generator == GeneratorName)
                .OrderByDescending(x => x.Date).ThenByDescending(x => x.CumFuelVolumeSinceLastReading)
                .Skip(1).FirstOrDefault();

            if(LastRowRunningHrs != null && PenultimateRowRunningHrs != null)
            {
                CumFuelVol = LastRowRunningHrs.CumFuelVolumeSinceLastReading;
                GenRunningHrs = LastRowRunningHrs.RunningHours;

                CurrFuelComp = (LastRowRunningHrs.CumFuelVolumeSinceLastReading
                                - PenultimateRowRunningHrs.CumFuelVolumeSinceLastReading) /
                                (LastRowRunningHrs.RunningHours 
                                - PenultimateRowRunningHrs.RunningHours);

                if (!(CurrFuelComp >= 0 && CurrFuelComp <= 100))CurrFuelComp = 0;
                if (!(StndFuelComp >= 0 && StndFuelComp <= 100)) StndFuelComp = 0;
                if (!(TestFuelComp >= 0 && TestFuelComp <= 100)) TestFuelComp = 0;

                if (CurrFuelComp < 0)
                {
                    MessageBox.Show($"Negative current fuel consumption " +
                                    $"rate for {GeneratorName}! " +
                                    "\n\nPlease check your last" +
                                    " two fuel consumption data",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);

                    CurrFuelComp = 0;
                }
            }
            else
            {
                CumFuelVol = 0;
                GenRunningHrs = 0;
                CurrFuelComp = 0;
            }

            return (CumFuelVol, GenRunningHrs, CurrFuelComp, StndFuelComp, TestFuelComp);
        }

    }
}
