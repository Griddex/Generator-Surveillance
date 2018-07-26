using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Interfaces
{
    public interface IGeneratorSchedulerRepository : IRepository<GeneratorScheduler>
    {
        ObservableCollection<GeneratorScheduler> GetAllGeneratorSchedules();

        ObservableCollection<GeneratorScheduler> GetActiveGeneratorSchedules();

        ObservableCollection<GeneratorScheduler> GetAllActiveGeneratorSchedules();

        double GetReminderInHrs(string GeneratorName, 
            ObservableCollection<GeneratorScheduler> AllGeneratorSchedules);

        DateTime GetStartDate(string GeneratorName, 
            ObservableCollection<GeneratorScheduler> AllGeneratorSchedules);

        string GetReminderLevel(string GeneratorName, 
            ObservableCollection<GeneratorScheduler> AllGeneratorSchedules);

        List<string> GetAllAuthorisers(string GeneratorName, 
            ObservableCollection<GeneratorScheduler> AllGeneratorSchedules);

        string GetAuthoriser(string GeneratorName, 
            ObservableCollection<GeneratorScheduler> AllGeneratorSchedules);

        void ActivateReminderNotification(string GeneratorName, DateTime StartDate,  
            double EveryHrs, string ReminderLevel, string RepeatReminderYesNo, string Authoriser);
        
        ObservableCollection<GeneratorScheduler> GetAllScheduledReminders();

        ObservableCollection<GeneratorScheduler> GetAnyPageGeneratorScheduledRmdrs(
            int pageIndex = 1, int pageSize = 10);

        Dictionary<string, int> GetActiveGeneratorsAndLastID();

        void GenerateNextReminders(string GeneratorName);

        void DeleteInactiveReminders();
    }
}
