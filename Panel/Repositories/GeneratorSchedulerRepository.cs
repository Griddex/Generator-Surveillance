using Panel.Interfaces;
using Panel.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Repositories
{
    public class GeneratorSchedulerRepository : Repository<GeneratorScheduler>, IGeneratorSchedulerRepository
    {
        public GeneratorSchedulerRepository(GeneratorSurveillanceDBEntities context) : base(context)
        {

        }

        public GeneratorSurveillanceDBEntities GeneratorSurveillanceDBContext
        {
            get { return Context as GeneratorSurveillanceDBEntities; }
        }

        public ObservableCollection<GeneratorScheduler> GetAllGeneratorSchedules()
        {
            return new ObservableCollection<GeneratorScheduler>
                (
                      GeneratorSurveillanceDBContext.GeneratorSchedulers
                     .Where(x => x.Id >= 0)                     
                 );
        }

        public double GetReminderInHrs(string GeneratorName, ObservableCollection<GeneratorScheduler> AllGeneratorSchedules)
        {
            return AllGeneratorSchedules
                    .Where(x => x.GeneratorName == GeneratorName)
                    .Select(x => x.EveryHrs)
                    .LastOrDefault();
        }

        public double GetNotificationTiming(string GeneratorName, ObservableCollection<GeneratorScheduler> AllGeneratorSchedules)
        {
            return AllGeneratorSchedules
                    .Where(x => x.GeneratorName == GeneratorName)
                    .Select(x => x.HrsThreshold)
                    .LastOrDefault();
        }

        public string GetAuthorizer(string GeneratorName, ObservableCollection<GeneratorScheduler> AllGeneratorSchedules)
        {
            return AllGeneratorSchedules
                    .Where(x => x.GeneratorName == GeneratorName)
                    .Select(x => x.Authorizer)
                    .LastOrDefault();
        }

        public void AddReminderNotification(string GeneratorName, double Reminder, double Notification, string Authorizer)
        {
            int NoOfRecords = GeneratorSurveillanceDBContext.GeneratorSchedulers.Count();
            GeneratorSurveillanceDBContext.GeneratorSchedulers.Add
            (
                new GeneratorScheduler
                {
                    Id = NoOfRecords,
                    GeneratorName = GeneratorName,
                    EveryHrs = Reminder,
                    HrsThreshold = Notification,
                    Authorizer = Authorizer
                }
            );
        }

        public ObservableCollection<GeneratorScheduler> GetAllScheduledReminders()
        {
            var AllGeneratorScheduledReminders = new ObservableCollection<GeneratorScheduler>
                                    (
                                    GeneratorSurveillanceDBContext.GeneratorSchedulers
                                    .AsParallel<GeneratorScheduler>()
                                    );
            return AllGeneratorScheduledReminders;
        }
    }
}
