using Panel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Repositories
{
    public class GeneratorUsageRepository : Repository<GeneratorUsage>, IGeneratorUsageRepository
    {
        public GeneratorUsageRepository(GeneratorSurveillanceDBEntities context) : base(context)
        {
            
        }

        public GeneratorSurveillanceDBEntities GeneratorSurveillanceDBContext
        {
            get { return Context as GeneratorSurveillanceDBEntities; }
        }

        
        public void GeneratorStarted(DateTime RecordDate, string GeneratorName, DateTime GeneratorStarted)
        {
            int NoOfRecords = GeneratorSurveillanceDBContext.GeneratorUsages.Count();
            GeneratorSurveillanceDBContext.GeneratorUsages.Add
            (
                new GeneratorUsage
                {
                    Id = NoOfRecords,
                    Date = RecordDate,
                    GeneratorName = GeneratorName,
                    GeneratorStarted = GeneratorStarted,
                    IsArchived = "No"
                }
            );
        }

        public void GeneratorStopped(DateTime GeneratorStoppedDate)
        {
            int NoOfRecords = GeneratorSurveillanceDBContext.GeneratorUsages.Count();
            var LastGeneratorUsageRecord = GeneratorSurveillanceDBContext.GeneratorUsages
                                            .SingleOrDefault(x => x.Id == NoOfRecords - 1);
            LastGeneratorUsageRecord.GeneratorStopped = GeneratorStoppedDate;
        }

        public ObservableCollection<GeneratorUsage> GetAllGeneratorUsages()
        {
            var AllGeneratorUsages = new ObservableCollection<GeneratorUsage>
                                    (
                                    GeneratorSurveillanceDBContext.GeneratorUsages
                                    .AsParallel<GeneratorUsage>() 
                                    );
            return AllGeneratorUsages;
        }

        public DateTime GetStartedDate(string GeneratorName)
        {
            var genFirstStartedDate = GeneratorSurveillanceDBContext
                                            .GeneratorUsages
                                            .FirstOrDefault(x => x.GeneratorName == GeneratorName)
                                            .Date;
            return genFirstStartedDate;
        }

        public DateTime GetStoppedDate(string GeneratorName)
        {
            DateTime genLastStoppedDate = GeneratorSurveillanceDBContext
                                            .GeneratorUsages
                                            .OrderByDescending(x => x.Date)
                                            .Where(x => x.GeneratorName == GeneratorName)
                                            .Select(x => x.Date)
                                            .FirstOrDefault();
            return genLastStoppedDate;
        }

        public DateTime GetStartedTime(string GeneratorName)
        {
            var genFirstStartedTime = GeneratorSurveillanceDBContext
                                            .GeneratorUsages
                                            .FirstOrDefault(x => x.GeneratorName == GeneratorName)
                                            .GeneratorStarted;
            return genFirstStartedTime;
        }

        public DateTime GetStoppedTime(string GeneratorName)
        {
            DateTime genLastStoppedTime = GeneratorSurveillanceDBContext
                                            .GeneratorUsages
                                            .OrderByDescending(x => x.Date)
                                            .Where(x => x.GeneratorName == GeneratorName)
                                            .Select(x => x.GeneratorStopped)
                                            .FirstOrDefault();
            return genLastStoppedTime;
        }
    }
}
