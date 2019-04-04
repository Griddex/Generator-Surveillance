using Panel.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Panel.Repositories
{
    public class GeneratorMaintenanceRepository : Repository<GeneratorMaintenance>, 
        IGeneratorMaintenanceRepository
    {
        public GeneratorMaintenanceRepository(GeneratorSurveillanceDBEntities context) 
            : base(context)
        {

        }

        public GeneratorSurveillanceDBEntities GeneratorSurveillanceDBContext
        {
            get { return Context as GeneratorSurveillanceDBEntities; }
        }

        public void AddUnschMaintenance(string MaintenanceType, DateTime UnschMaintenancedate,
            string UnschComments, double UnschTotalCost, string GeneratorName)
        {
            int RecordNo = GeneratorSurveillanceDBContext.GeneratorMaintenances
                 .OrderByDescending(x => x.Id).Select(x => x.Id).ToArray()[0];

            int SNNo = GeneratorSurveillanceDBContext.GeneratorMaintenances
                .OrderByDescending(x => x.SN).Select(x => x.SN).ToArray()[0];

            GeneratorSurveillanceDBContext.GeneratorMaintenances.Add
            (
                new GeneratorMaintenance
                {
                    Id = RecordNo + 1,
                    SN = SNNo + 1,
                    GeneratorName = GeneratorName,
                    Date = UnschMaintenancedate,
                    MaintenanceType = MaintenanceType,
                    Comments = UnschComments,
                    TotalCost = UnschTotalCost
                }
            );
        }

        public void AddSchMaintenance(string MaintenanceType, DateTime SchMaintenancedate,
            string SchComments, double SchTotalCost, string GeneratorName)
        {
            int RecordNo = GeneratorSurveillanceDBContext.GeneratorMaintenances
                 .OrderByDescending(x => x.Id).Select(x => x.Id).ToArray()[0];

            int SNNo = GeneratorSurveillanceDBContext.GeneratorMaintenances
                .OrderByDescending(x => x.SN).Select(x => x.SN).ToArray()[0];

            GeneratorSurveillanceDBContext.GeneratorMaintenances.Add
            (
                new GeneratorMaintenance
                {
                    Id = RecordNo + 1,
                    SN = SNNo + 1,
                    GeneratorName = GeneratorName,
                    Date = SchMaintenancedate,
                    MaintenanceType = MaintenanceType,
                    Comments = SchComments,
                    TotalCost = SchTotalCost
                }
            );
        }

        public ObservableCollection<GeneratorMaintenance> GetAllGeneratorMaintenances()
        {
            var AllGeneratorMaintenance =   new ObservableCollection<GeneratorMaintenance>
            (
            GeneratorSurveillanceDBContext.GeneratorMaintenances
            .AsParallel<GeneratorMaintenance>()
            );

            return AllGeneratorMaintenance;
        }

        public ObservableCollection<GeneratorMaintenance> GetAnyPageGeneratorMaintenance(int pageIndex = 1, 
            int pageSize = 10)
        {
            var FirstPageGeneratorMaintenance = new ObservableCollection<GeneratorMaintenance>
            (
                GeneratorSurveillanceDBContext.GeneratorMaintenances
                .OrderBy(x => x.SN)
                .Skip((pageIndex - 1) * pageSize)
                .Take(10)
                .AsParallel<GeneratorMaintenance>()
            );

            return FirstPageGeneratorMaintenance;
        }

    }
}
