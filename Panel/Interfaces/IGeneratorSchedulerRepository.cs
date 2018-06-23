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
        double GetReminderInHrs(string GeneratorName, ObservableCollection<GeneratorScheduler> AllGeneratorSchedules);
        DateTime GetStartDate(string GeneratorName, ObservableCollection<GeneratorScheduler> AllGeneratorSchedules);
        string GetReminderLevel(string GeneratorName, ObservableCollection<GeneratorScheduler> AllGeneratorSchedules);
        List<string> GetAllAuthorizers(string GeneratorName, ObservableCollection<GeneratorScheduler> AllGeneratorSchedules);
        string GetAuthorizer(string GeneratorName, ObservableCollection<GeneratorScheduler> AllGeneratorSchedules);
        void ActivateReminderNotification(string GeneratorName, DateTime StartDate,  double EveryHrs, string ReminderLevel, string Authorizer);
        
        ObservableCollection<GeneratorScheduler> GetAllScheduledReminders();
    }
}
