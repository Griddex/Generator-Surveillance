using Panel.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        public static GeneratorScheduler NextGeneratorForNotification { get; set; }
        public static GeneratorScheduler NextGeneratorForNotifications { get; set; }
        public static ObservableCollection<GeneratorScheduler> AllGeneratorSchedules { get; set; }
        public static ObservableCollection<GeneratorMaintenance> GetAllGeneratorMaintenances { get; set; }


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
        public static string MaintenanceDeliverable { get; set; }

        public static Dictionary<string, int> GeneratorAndLastIDDict { get; set; }
        public static GeneratorSchedulerRepository GeneratorSchedulerRepository { get; set; }

        public static UnityContainer container = (UnityContainer)Application.Current.Resources["UnityIoC"];
        public static GeneratorSurveillanceDBEntities GSE {get; set; } = container.Resolve<GeneratorSurveillanceDBEntities>();
        public static GeneratorSchedulerRepository GSR { get; set; } = new GeneratorSchedulerRepository(GSE);
        public static GeneratorMaintenanceRepository GMR { get; set; } = new GeneratorMaintenanceRepository(GSE);

        public static async void Initialise()
        {
            GeneratorSurveillanceDBEntities GSE  = container.Resolve<GeneratorSurveillanceDBEntities>();
            GeneratorSchedulerRepository GSR  = new GeneratorSchedulerRepository(GSE);

            TimeSpan CheckDuration = NotificationTimerInterval.Interval;
            GeneratorSchedulerRepository = GSR;
            AllGeneratorSchedules = GSR.GetAllGeneratorSchedules();
            GetAllGeneratorMaintenances = GMR.GetAllGeneratorMaintenances();

            if(GetAllGeneratorMaintenances.Count != 0 && GeneratorName != null)
            {
                MaintenanceDeliverable = GetAllGeneratorMaintenances
                .Where(x => x.GeneratorName == GeneratorName)
                .OrderByDescending(x => x.Date).FirstOrDefault().Comments;
            }            

            DateTime dateTime = DateTime.Now;
            List<GeneratorScheduler> NextGeneratorForNotifications = AllGeneratorSchedules
                          .Where(x => x.IsActive == "Yes")
                          .Where(x => x.ReminderDateTimeProfile > dateTime)
                          .Where(x => x.ReminderDateTimeProfile < dateTime + CheckDuration)
                          .OrderBy(x => x.ReminderDateTimeProfile - dateTime)
                          .ToList();

            int NextGeneratorForNotificationsCount = NextGeneratorForNotifications.Count();
            if (NextGeneratorForNotificationsCount != 0)
            {
                List<Task> NotificationTasks = new List<Task>();
                foreach (var item in NextGeneratorForNotifications)
                {

                    int GenID = GeneratorID;
                    string GenName = GeneratorName;
                    string RemLevel = ReminderLevel;
                    int FstID = FirstID;
                    int LstID = LastID;
                    DateTime FlNotfnDate = FinalNotificationDate;
                    TimeSpan NtNotfnDate = NextNotificationDuration;
              
                    NotificationTasks.Add(Task.Run
                    (async () =>
                        {
                            int GeneratorID = item.SN;
                            string GeneratorName = item.GeneratorName;
                            string ReminderLevel = item.ReminderLevel;

                            int FirstID = AllGeneratorSchedules
                                        .Where(x => x.GeneratorName ==
                                                    item.GeneratorName)
                                        .Where(x => x.IsActive == "Yes")
                                        .OrderBy(x => x.SN)
                                        .FirstOrDefault().SN;

                            int LastID = AllGeneratorSchedules
                                        .Where(x => x.GeneratorName ==
                                                    item.GeneratorName)
                                        .Where(x => x.IsActive == "Yes")
                                        .OrderBy(x => x.SN)
                                        .LastOrDefault().SN;

                            DateTime FinalNotificationDate = AllGeneratorSchedules
                                                    .Where(x => x.GeneratorName ==
                                                                item.GeneratorName)
                                                    .Where(x => x.IsActive == "Yes")
                                                    .Where(x => x.ReminderDateTimeProfile > dateTime)
                                                    .OrderByDescending(x => dateTime - x.ReminderDateTimeProfile)
                                                    .LastOrDefault().ReminderDateTimeProfile;

                            TimeSpan NextNotificationDuration = item.ReminderDateTimeProfile - dateTime;

                            double SecondsFromNextNotification = NextNotificationDuration.TotalSeconds;

                            await Task.Delay
                                  (Convert.ToInt32(SecondsFromNextNotification) * 1000);

                            SendNotification(GeneratorName, ReminderLevel, NextNotificationDuration, 
                                            FinalNotificationDate, FirstID, LastID, GeneratorID,
                                            MaintenanceDeliverable);

                            Debug.Print(Convert.ToString(GeneratorID));
                            Debug.Print(Convert.ToString(FirstID));
                            Debug.Print(Convert.ToString(LastID));
                            Debug.Print(Convert.ToString(GeneratorName));
                            Debug.Print(Convert.ToString(ReminderLevel));                            
                            Debug.Print(Convert.ToString(FinalNotificationDate));
                            Debug.Print(Convert.ToString(NextNotificationDuration));
                        }
                    ));
                }
                await Task.WhenAll(NotificationTasks);
            }
        }

        private static void SendNotification(string GeneratorName, string ReminderLevel, 
                                            TimeSpan NextNotificationDuration, 
                                            DateTime FinalNotificationDate, int FirstID,
                                            int LastID, int GeneratorID,
                                            string MaintenanceDeliverable)
        {
            EmailService emailService = new EmailService();

            emailService.SendMessage(GeneratorName, 
                                    ReminderLevel, 
                                    NextNotificationDuration, 
                                    FinalNotificationDate,
                                    FirstID, 
                                    LastID, 
                                    GeneratorID,
                                    MaintenanceDeliverable);
        }
    }
}
