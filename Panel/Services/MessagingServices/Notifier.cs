using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using TimerTimer = System.Timers.Timer;
using Panel.Repositories;
using System.Collections.ObjectModel;
using Unity;

namespace Panel.Services.MessagingServices
{
    public static class Notifier
    {
        public static GeneratorScheduler 
            NextGeneratorForNotification { get; set; }
        public static ObservableCollection<GeneratorScheduler> 
            AllGeneratorSchedules { get; set; }
        public static Timer Timer { get; set; }
        public static TimerTimer timer { get; set; }
        public static string GeneratorName { get; set; }
        public static string ReminderLevel { get; set; }
        public static TimeSpan NextNotificationDuration { get; set; }
        public static string NotificationTime { get; set; }
        public static DateTime FinalNotificationDate { get; set; }
        public static int FirstID { get; set; }
        public static int LastID { get; set; }
        public static int GeneratorID { get; set; }
        public static Dictionary<string,int> 
            GeneratorAndLastIDDict { get; set; }
        public static GeneratorSchedulerRepository 
            GeneratorSchedulerRepository { get; set; }

        public static UnityContainer container  = 
            (UnityContainer)Application.Current.Resources["UnityIoC"];

        public static void Initialise()
        {
            var gse = container.Resolve<GeneratorSurveillanceDBEntities>();
            var gsr = new GeneratorSchedulerRepository(gse);
            GeneratorSchedulerRepository = gsr;

            AllGeneratorSchedules = gsr.GetAllGeneratorSchedules();
            var CurrentActiveGenerators = gsr.GetActiveGeneratorSchedules();

            NextGeneratorForNotification = AllGeneratorSchedules
                                        .Where(x => x.IsActive == "Yes")
                                        .Where(x => x.ReminderDateTimeProfile > DateTime.Now)
                                        .OrderBy(x => x.ReminderDateTimeProfile - DateTime.Now)
                                        .FirstOrDefault();

            GeneratorID = NextGeneratorForNotification.Id;
            GeneratorName = NextGeneratorForNotification.GeneratorName;
            ReminderLevel = NextGeneratorForNotification.ReminderLevel;

            FinalNotificationDate = AllGeneratorSchedules
                                    .Where(x => x.GeneratorName == NextGeneratorForNotification
                                                                        .GeneratorName)
                                    .Where(x => x.IsActive == "Yes")
                                    .Where(x => x.ReminderDateTimeProfile > DateTime.Now)
                                    .OrderBy(x => DateTime.Now - x.ReminderDateTimeProfile)
                                    .LastOrDefault().ReminderDateTimeProfile;
            NextNotificationDuration = NextGeneratorForNotification
                                            .ReminderDateTimeProfile - DateTime.Now;

            FirstID = AllGeneratorSchedules
                        .Where(x => x.GeneratorName == NextGeneratorForNotification
                                                                .GeneratorName)
                        .Where(x => x.IsActive == "Yes")
                        .OrderBy(x => x.Id)
                        .FirstOrDefault().Id;
            LastID = AllGeneratorSchedules
                        .Where(x => x.GeneratorName == NextGeneratorForNotification
                                                                .GeneratorName)
                        .Where(x => x.IsActive == "Yes")
                        .OrderBy(x => x.Id)
                        .LastOrDefault().Id;
            
            int SecondsFromNextNotification = (int)(NextNotificationDuration
                                                                .TotalSeconds);

            TimerTimer timer = new TimerTimer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = SecondsFromNextNotification * 1000;
            timer.AutoReset = false;
            timer.Enabled = true;

            GeneratorAndLastIDDict = gsr.GetActiveGeneratorsAndLastID();
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            EmailService emailService = new EmailService();
            emailService.SendMessage(GeneratorName, ReminderLevel, NotificationTime,
                                        NextNotificationDuration, FinalNotificationDate,
                                        FirstID, LastID, GeneratorID);
            if(timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }

            foreach (var genid in GeneratorAndLastIDDict)
            {
                if(GeneratorName == genid.Key && 
                    LastID == genid.Value)
                {
                    GeneratorSchedulerRepository
                        .GenerateNextReminders(GeneratorName);
                }
            }

            Initialise();
        }
    }
}
