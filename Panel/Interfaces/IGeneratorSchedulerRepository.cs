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
        double GetReminderInHrs(string GeneratorName, ObservableCollection<GeneratorScheduler> AllGeneratorSchedules);
        double GetNotificationTiming(string GeneratorName, ObservableCollection<GeneratorScheduler> AllGeneratorSchedules);
        void AddReminderNotification(string GeneratorName, double Reminder, double Notification, string Authorizer);
        string GetAuthorizer(string GeneratorName, ObservableCollection<GeneratorScheduler> AllGeneratorSchedules);
        ObservableCollection<GeneratorScheduler> GetAllScheduledReminders();
    }
}
