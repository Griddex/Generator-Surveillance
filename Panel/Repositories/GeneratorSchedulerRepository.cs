using Panel.BusinessLogic.MaintenanceLogic;
using Panel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Panel.Repositories
{
    public class GeneratorSchedulerRepository : Repository<GeneratorScheduler>, 
        IGeneratorSchedulerRepository
    {
        private Dictionary<string, int> GeneratorAndLastIDDict =
            new Dictionary<string, int>();
        private GeneratorScheduler GenLastRowReminder;

        public GeneratorSchedulerRepository(GeneratorSurveillanceDBEntities context) 
            : base(context) { }

        public GeneratorSurveillanceDBEntities GeneratorSurveillanceDBContext
        {
            get { return Context as GeneratorSurveillanceDBEntities; }
        }

        public ObservableCollection<GeneratorScheduler> GetAllGeneratorSchedules()
        {
            return new ObservableCollection<GeneratorScheduler>
            (
                    GeneratorSurveillanceDBContext
                    .GeneratorSchedulers
                    .Where(x => x.SN >= 0)                     
            );
        }

        public ObservableCollection<GeneratorScheduler> GetActiveGeneratorSchedules()
        {
            return new ObservableCollection<GeneratorScheduler>
            (
                    GeneratorSurveillanceDBContext
                    .GeneratorSchedulers
                    .Where(x => x.SN >= 0 && x.IsActive == "Yes")
                    .GroupBy(x => x.GeneratorName, 
                            (Key,g) => g.FirstOrDefault())                         
            );
        }

        public ObservableCollection<GeneratorScheduler> GetAllActiveGeneratorSchedules()
        {
            return new ObservableCollection<GeneratorScheduler>
            (
                    GeneratorSurveillanceDBContext
                    .GeneratorSchedulers
                    .Where(x => x.SN >= 0 && x.IsActive == "Yes")                         
            );
        }

        public DateTime GetStartDate(string GeneratorName, 
            ObservableCollection<GeneratorScheduler> AllGeneratorSchedules)
        {
            return AllGeneratorSchedules
                    .Where(x => x.GeneratorName == GeneratorName)
                    .Where(x => x.IsActive == "Yes")
                    .Select(x => x.Starts)
                    .LastOrDefault();
        }

        public double GetReminderInHrs(string GeneratorName, 
            ObservableCollection<GeneratorScheduler> AllGeneratorSchedules)
        {
            return AllGeneratorSchedules
                    .Where(x => x.GeneratorName == GeneratorName)
                    .Where(x => x.IsActive == "Yes")
                    .Select(x => x.Every)
                    .LastOrDefault();
        }

        public string GetReminderLevel(string GeneratorName, 
            ObservableCollection<GeneratorScheduler> AllGeneratorSchedules)
        {
            return AllGeneratorSchedules
                    .Where(x => x.GeneratorName == GeneratorName)
                    .Where(x => x.IsActive == "Yes")
                    .Select(x => x.ReminderLevel)
                    .LastOrDefault();
        }

        public List<string> GetAllAuthorisers(string GeneratorName, 
            ObservableCollection<GeneratorScheduler> AllGeneratorSchedules)
        {
            return AllGeneratorSchedules
                    .Where(x => x.GeneratorName == GeneratorName)
                    .Where(x => x.IsActive == "Yes")
                    .Select(x => x.Authoriser)
                    .Distinct().ToList();
        }

        public string GetAuthoriser(string GeneratorName, 
            ObservableCollection<GeneratorScheduler> AllGeneratorSchedules)
        {
            return AllGeneratorSchedules
                    .Where(x => x.GeneratorName == GeneratorName)
                    .Where(x => x.IsActive == "Yes")
                    .Select(x => x.Authoriser)
                    .LastOrDefault();
        }

        public void ActivateReminderNotification(string GeneratorName, DateTime StartDate, double EveryHrs, 
            string ReminderLevel, string RepeatReminderYesNo, string Authoriser)
        {
            foreach (var row in GeneratorSurveillanceDBContext.GeneratorSchedulers)
            {
                if (row.GeneratorName == GeneratorName && row.IsActive == "Yes")
                    row.IsActive = "No";
            }

            Tuple<List<double>, List<DateTime>> NotificationHoursDateTime = ScheduledMaintenanceNotificationLogic
                .GenerateNotificationHoursAndDates(StartDate, EveryHrs, ReminderLevel);

            int i = 1;            
            int RecordNo = GeneratorSurveillanceDBContext
                                    .GeneratorSchedulers
                                    .Count();           
                                            
            foreach (double Hours in NotificationHoursDateTime.Item1)
            {
                GeneratorSurveillanceDBContext.GeneratorSchedulers.Add
                (
                    new GeneratorScheduler
                    {
                        Id = RecordNo,
                        SN = RecordNo + i,
                        GeneratorName = GeneratorName,
                        Starts = StartDate,
                        Every = EveryHrs,
                        ReminderLevel = ReminderLevel,
                        Authoriser = Authoriser,
                        IsActive = "Yes",
                        IsRepetitive = RepeatReminderYesNo,
                        ReminderHoursProfile = Hours,
                        ReminderDateTimeProfile = NotificationHoursDateTime
                                                    .Item2
                                                    .ElementAt(i - 1)
                    }
                );
                i++;
            }           
        }

        public ObservableCollection<GeneratorScheduler> GetAllScheduledReminders()
        {
            return new ObservableCollection<GeneratorScheduler>
                    (
                    GeneratorSurveillanceDBContext
                    .GeneratorSchedulers
                    .AsParallel<GeneratorScheduler>()
                    );         
        }

        public ObservableCollection<GeneratorScheduler> GetAnyPageGeneratorScheduledRmdrs(
            int pageIndex = 1, int pageSize = 10)
        {
            int RecordNo = GeneratorSurveillanceDBContext
                                    .GeneratorSchedulers
                                    .Count();

            var NextPageLastRowNumber = pageIndex * pageSize;
            int SkipBy = (pageIndex == 1) ? (pageIndex - 1) * pageSize
                                          : ((pageIndex - 1) * pageSize) - 1;
            if ((RecordNo - NextPageLastRowNumber) > pageSize)
            {
                return new ObservableCollection<GeneratorScheduler>
                        (
                            GeneratorSurveillanceDBContext.GeneratorSchedulers
                            .OrderBy(x => x.SN)
                            .Skip(SkipBy)
                            .Take(pageSize)
                            .AsParallel<GeneratorScheduler>()
                        );

            }
            else
            {
                return new ObservableCollection<GeneratorScheduler>
                        (
                            GeneratorSurveillanceDBContext.GeneratorSchedulers
                            .OrderBy(x => x.SN)
                            .Skip(SkipBy)
                            .Take(pageSize)
                            .AsParallel<GeneratorScheduler>()
                        );
            }
        }

        public Dictionary<string,int> GetActiveGeneratorsAndLastID()
        {
            List<string> Generators = GeneratorSurveillanceDBContext
                                                .GeneratorSchedulers
                                                .Where(x => x.IsActive == "Yes")
                                                .Where(x => x.IsRepetitive == "Yes")
                                                .Select(x =>  x.GeneratorName)
                                                .Distinct().ToList();

            ObservableCollection<GeneratorScheduler>
                ActiveGeneratorsAndLastRow = new ObservableCollection<GeneratorScheduler>
                                             (
                                                GeneratorSurveillanceDBContext
                                                .GeneratorSchedulers
                                                .Where(x => x.SN >= 0 && 
                                                            x.IsActive == "Yes" && 
                                                            x.IsRepetitive == "Yes")
                                                .GroupBy(x => x.GeneratorName, 
                                                        (Key, g) => g.OrderByDescending(c => c.SN)
                                                                        .FirstOrDefault())
                                             );

            foreach (var gen in Generators)
            {
                foreach (var row in ActiveGeneratorsAndLastRow)
                {
                    if(gen == row.GeneratorName)
                    {
                        GeneratorAndLastIDDict.Add(gen, row.SN);
                        break;
                    }
                }
            }

            return GeneratorAndLastIDDict;
        }

        public void GenerateNextReminders(string GeneratorName)
        {
            foreach (var row in GeneratorSurveillanceDBContext
                                        .GeneratorSchedulers)
            {
                if (row.GeneratorName == GeneratorName &&
                    row.IsRepetitive == "Yes")
                {
                    GenLastRowReminder = row;
                    row.IsActive = "No";
                    row.IsRepetitive = "No";
                }
            }

            DateTime NextStartDate = GenLastRowReminder.Starts
                                                       .AddHours(GenLastRowReminder
                                                                .Every);

            ActivateReminderNotification(GeneratorName, NextStartDate,
                     GenLastRowReminder.Every, 
                     GenLastRowReminder.ReminderLevel,
                     GenLastRowReminder.IsRepetitive = "Yes", 
                     GenLastRowReminder.Authoriser);
        }

        public void DeleteInactiveReminders()
        {
            var InactiveReminders = GeneratorSurveillanceDBContext
                                        .GeneratorSchedulers
                                        .Where(x => x.SN >= 0 &&
                                                    x.IsActive == "No")
                                        .ToList();

            RemoveRange
            (
                new ObservableCollection<GeneratorScheduler>
                (
                    InactiveReminders
                )
            );
        }

        public List<string> GetUniqueAuthorisers()
        {
            var UniqueAuthorisers =
                GeneratorSurveillanceDBContext
                                .GeneratorSchedulers
                                .Select(x => x.Authoriser)
                                .Distinct()
                                .ToList();

            return UniqueAuthorisers;
        }
    }
}
