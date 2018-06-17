using Panel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Repositories
{
    public class GeneratorFuellingRepository : Repository<GeneratorFuelling>, IGeneratorFuellingRepository
    {
        public GeneratorFuellingRepository(GeneratorSurveillanceDBEntities context) : base(context)
        {

        }

        public GeneratorSurveillanceDBEntities GeneratorSurveillanceDBContext
        {
            get { return Context as GeneratorSurveillanceDBEntities; }
        }

        public void AddFuelPurchaseRecord(DateTime Fuellingdate, string Vendor, double Volumeofdiesel, double Costofdiesel)
        {
            int NoOfRecords = GeneratorSurveillanceDBContext.GeneratorFuellings.Count();
            GeneratorSurveillanceDBContext.GeneratorFuellings.Add
            (
                new GeneratorFuelling
                {
                    Id = NoOfRecords + 1,
                    Date = Fuellingdate,
                    Vendor = Vendor,
                    VolumeOfDiesel = Volumeofdiesel,
                    CostOfDiesel = Costofdiesel
                }
            );
        }

        public void AddFuelConsumptionHours(string GeneratorName, DateTime RunningHoursDate, double RunningHours,
                                        double CumFuelVolumeSinceLastReading)
        {
            int NoOfRecords = GeneratorSurveillanceDBContext.GeneratorRunningHrs.Count();
            GeneratorSurveillanceDBContext.GeneratorRunningHrs.Add
            (
                new GeneratorRunningHr
                {
                    Id = NoOfRecords + 1,
                    Generator = GeneratorName,
                    Date = RunningHoursDate,
                    CumFuelVolumeSinceLastReading = CumFuelVolumeSinceLastReading,
                    RunningHours = RunningHours
                }
            );
        }

        public ObservableCollection<GeneratorFuelling> GetAllGeneratorFuellings()
        {
            var AllGeneratorFuellings = new ObservableCollection<GeneratorFuelling>
                                    (
                                    GeneratorSurveillanceDBContext.GeneratorFuellings
                                    .AsParallel<GeneratorFuelling>()
                                    );
            return AllGeneratorFuellings;
        }
    }
}
