using Panel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void AddUnschMaintenance(string MaintenanceType, 
                                        DateTime UnschMaintenancedate, 
                                        string UnschComments, 
                                        double UnschTotalCost)
        {
            int NoOfRecords = GeneratorSurveillanceDBContext
                                        .GeneratorMaintenances
                                        .Count();
            GeneratorSurveillanceDBContext.GeneratorMaintenances
                                          .Add
            (
                new GeneratorMaintenance
                {
                    Id = NoOfRecords,
                    Date = UnschMaintenancedate,
                    MaintenanceType = MaintenanceType,
                    Comments = UnschComments,
                    TotalCost = UnschTotalCost
                }
            );
        }

        public void AddSchMaintenance(string MaintenanceType, 
                                      DateTime SchMaintenancedate, 
                                      string SchComments, 
                                      double SchTotalCost)
        {
            int NoOfRecords = GeneratorSurveillanceDBContext
                                        .GeneratorMaintenances
                                        .Count();
            GeneratorSurveillanceDBContext.GeneratorMaintenances
                                          .Add
            (
                new GeneratorMaintenance
                {
                    Id = NoOfRecords,
                    Date = SchMaintenancedate,
                    MaintenanceType = MaintenanceType,
                    Comments = SchComments,
                    TotalCost = SchTotalCost
                }
            );
        }

        public ObservableCollection<GeneratorMaintenance> 
            GetAllGeneratorMaintenances()
        {
            var AllGeneratorMaintenance = 
                    new ObservableCollection<GeneratorMaintenance>
                    (
                    GeneratorSurveillanceDBContext.GeneratorMaintenances
                    .AsParallel<GeneratorMaintenance>()
                    );
            return AllGeneratorMaintenance;
        }

        public ObservableCollection<GeneratorMaintenance> 
            GetAnyPageGeneratorMaintenance(int pageIndex = 1, 
                                           int pageSize = 10)
        {
            var FirstPageGeneratorMaintenance = 
                    new ObservableCollection<GeneratorMaintenance>
                    (
                        GeneratorSurveillanceDBContext.GeneratorMaintenances
                        .OrderBy(x => x.Id)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(10)
                        .AsParallel<GeneratorMaintenance>()
                    );
            return FirstPageGeneratorMaintenance;
        }

    }
}
