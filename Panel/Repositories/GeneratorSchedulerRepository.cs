using Panel.BusinessLogic.MaintenanceLogic;
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

        public ObservableCollection<GeneratorScheduler> GetActiveGeneratorSchedules()
        {
            return new ObservableCollection<GeneratorScheduler>
                    (
                          GeneratorSurveillanceDBContext.GeneratorSchedulers
                         .Where(x => x.Id >= 0 && x.IsActive == "Yes")
                         .OrderBy(x => x.GeneratorName)                         
                     );
        }

        public DateTime GetStartDate(string GeneratorName, ObservableCollection<GeneratorScheduler> AllGeneratorSchedules)
        {
            return AllGeneratorSchedules
                    .Where(x => x.GeneratorName == GeneratorName)
                    .Where(x => x.IsActive == "Yes")
                    .Select(x => x.Starts)
                    .LastOrDefault();
        }

        public double GetReminderInHrs(string GeneratorName, ObservableCollection<GeneratorScheduler> AllGeneratorSchedules)
        {
            return AllGeneratorSchedules
                    .Where(x => x.GeneratorName == GeneratorName)
                    .Where(x => x.IsActive == "Yes")
                    .Select(x => x.Every)
                    .LastOrDefault();
        }

        public string GetReminderLevel(string GeneratorName, ObservableCollection<GeneratorScheduler> AllGeneratorSchedules)
        {
            return AllGeneratorSchedules
                    .Where(x => x.GeneratorName == GeneratorName)
                    .Where(x => x.IsActive == "Yes")
                    .Select(x => x.ReminderLevel)
                    .LastOrDefault();
        }

        public List<string> GetAllAuthorizers(string GeneratorName, ObservableCollection<GeneratorScheduler> AllGeneratorSchedules)
        {
            return AllGeneratorSchedules
                    .Where(x => x.GeneratorName == GeneratorName)
                    .Where(x => x.IsActive == "Yes")
                    .Select(x => x.Authorizer)
                    .Distinct().ToList();
        }

        public string GetAuthorizer(string GeneratorName, ObservableCollection<GeneratorScheduler> AllGeneratorSchedules)
        {
            return AllGeneratorSchedules
                    .Where(x => x.GeneratorName == GeneratorName)
                    .Where(x => x.IsActive == "Yes")
                    .Select(x => x.Authorizer)
                    .LastOrDefault();
        }

        public void ActivateReminderNotification(string GeneratorName, DateTime StartDate, double EveryHrs, string ReminderLevel, string Authorizer)
        {
            foreach (var row in GeneratorSurveillanceDBContext.GeneratorSchedulers)
            {
                if (row.GeneratorName == GeneratorName && row.IsActive == "Yes")
                    row.IsActive = "No";
            }

            Tuple<List<double>, List<DateTime>> NotificationHoursDateTime = ScheduledMaintenanceNotificationLogic
                                                                .GenerateNotificationHoursAndDates(StartDate, EveryHrs, ReminderLevel);

            int i = 0;
            int NoOfRecords = GeneratorSurveillanceDBContext.GeneratorSchedulers.Count();
            foreach (double Hours in NotificationHoursDateTime.Item1)
            {
                GeneratorSurveillanceDBContext.GeneratorSchedulers.Add
                (
                    new GeneratorScheduler
                    {
                        Id = NoOfRecords,
                        GeneratorName = GeneratorName,
                        Starts = StartDate,
                        Every = EveryHrs,
                        ReminderLevel = ReminderLevel,
                        Authorizer = Authorizer,
                        IsActive = "Yes",
                        ReminderHoursProfile = Hours,
                        ReminderDateTimeProfile = NotificationHoursDateTime.Item2.ElementAt(i)
                    }
                );
                i++;
            }           
        }

        public ObservableCollection<GeneratorScheduler> GetAllScheduledReminders()
        {
            return new ObservableCollection<GeneratorScheduler>
                    (
                    GeneratorSurveillanceDBContext.GeneratorSchedulers
                    .AsParallel<GeneratorScheduler>()
                    );         
        }
    }
}
