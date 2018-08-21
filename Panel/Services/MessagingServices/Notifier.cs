using Panel.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using TimerTimer = System.Timers.Timer;

namespace Panel.Services.MessagingServices
{
    public static class Notifier
    {
        public static GeneratorScheduler
            NextGeneratorForNotification { get; set; }

        public static GeneratorScheduler
            NextGeneratorForNotifications { get; set; }

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

        public static Dictionary<string, int>
            GeneratorAndLastIDDict { get; set; }

        public static GeneratorSchedulerRepository
            GeneratorSchedulerRepository { get; set; }

        public static UnityContainer container =
            (UnityContainer)Application.Current.Resources["UnityIoC"];

        public static GeneratorSurveillanceDBEntities GSE {get; set; } =
            container.Resolve<GeneratorSurveillanceDBEntities>();

        public static GeneratorSchedulerRepository GSR { get; set; } =
            new GeneratorSchedulerRepository(GSE);

        public static void Initialise()
        {
            GeneratorSurveillanceDBEntities GSE  =
                container.Resolve<GeneratorSurveillanceDBEntities>();

            GeneratorSchedulerRepository GSR  =
                new GeneratorSchedulerRepository(GSE);

            TimeSpan CheckDuration = new TimeSpan(0, 20, 0);
            GeneratorSchedulerRepository = GSR;
            AllGeneratorSchedules = GSR.GetAllGeneratorSchedules();

            DateTime dateTime = DateTime.Now;
            List<GeneratorScheduler> NextGeneratorForNotifications
                          = AllGeneratorSchedules
                          .Where(x => x.IsActive == "Yes")
                          .Where(x => x.ReminderDateTimeProfile > dateTime)
                          .Where(x => x.ReminderDateTimeProfile < dateTime + CheckDuration)
                          .OrderBy(x => x.ReminderDateTimeProfile - dateTime)
                          .ToList();

            int NextGeneratorForNotificationsCount = NextGeneratorForNotifications.Count();
            if (NextGeneratorForNotificationsCount != 0)
            {
                Task[] NotificationTasks = new Task[NextGeneratorForNotificationsCount];
                int i = 0;
                foreach (var item in NextGeneratorForNotifications)
                {
                    NotificationTasks[i] = Task.Run
                    (
                        async () =>
                        {
                            PrepareNotificationMetaData(dateTime, item);

                            double SecondsFromNextNotification =
                                NextNotificationDuration.TotalSeconds;

                            await Task.Delay
                                  (Convert.ToInt32(SecondsFromNextNotification)
                                    * 1000);

                            SendNotification();
                        }
                    );
                    i += 1;
                }
            }                
        }

        private static void PrepareNotificationMetaData(DateTime dateTime, 
            GeneratorScheduler Notification)
        {
            GeneratorID = Notification.Id;
            GeneratorName = Notification.GeneratorName;
            ReminderLevel = Notification.ReminderLevel;

            FirstID = AllGeneratorSchedules
                        .Where(x => x.GeneratorName ==
                                    Notification
                                    .GeneratorName)
                        .Where(x => x.IsActive == "Yes")
                        .OrderBy(x => x.Id)
                        .FirstOrDefault().Id;

            LastID = AllGeneratorSchedules
                        .Where(x => x.GeneratorName ==
                                    Notification
                                    .GeneratorName)
                        .Where(x => x.IsActive == "Yes")
                        .OrderBy(x => x.Id)
                        .LastOrDefault().Id;

            FinalNotificationDate = AllGeneratorSchedules
                                    .Where(x => x.GeneratorName ==
                                                Notification
                                                .GeneratorName)
                                    .Where(x => x.IsActive == "Yes")
                                    .Where(x => x.ReminderDateTimeProfile > dateTime)
                                    .OrderBy(x => dateTime - x.ReminderDateTimeProfile)
                                    .LastOrDefault().ReminderDateTimeProfile;

            NextNotificationDuration = Notification
                                       .ReminderDateTimeProfile - dateTime;
        }

        private static void SendNotification()
        {
            EmailService emailService = new EmailService();

            emailService.SendMessage(GeneratorName, 
                                    ReminderLevel, 
                                    NextNotificationDuration, 
                                    FinalNotificationDate,
                                    FirstID, 
                                    LastID, 
                                    GeneratorID);
        }
    }
}
