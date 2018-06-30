using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Panel.Repositories;
using System.Collections.ObjectModel;

namespace Panel.Services.MessagingServices
{
    public class Notifier
    {
        public GeneratorScheduler NextGeneratorForNotification { get; set; }
        public ObservableCollection<GeneratorScheduler> AllGeneratorSchedules { get; set; }
        public Timer Timer { get; set; }
        public string GeneratorName { get; set; }
        public string ReminderLevel { get; set; }
        public TimeSpan NextNotificationDuration { get; set; }
        public string NotificationTime { get; set; }
        public DateTime FinalNotificationDate { get; set; }
        public int FirstID { get; set; }
        public int LastID { get; set; }
        public int GeneratorID { get; set; }

        public Notifier()
        {
            Initialise();
        }

        public void Initialise()
        {
            GeneratorSchedulerRepository gs = new GeneratorSchedulerRepository
                                                (
                                                    new GeneratorSurveillanceDBEntities()
                                                );
            AllGeneratorSchedules = gs.GetAllGeneratorSchedules();
            var CurrentActiveGenerators = gs.GetActiveGeneratorSchedules();

            NextGeneratorForNotification = AllGeneratorSchedules
                                           .Where(x => x.IsActive == "Yes")
                                           .Where(x => x.ReminderDateTimeProfile > DateTime.Now)
                                           .OrderBy(x => x.ReminderDateTimeProfile - DateTime.Now)
                                           .FirstOrDefault();

            GeneratorID = NextGeneratorForNotification.Id;
            GeneratorName = NextGeneratorForNotification.GeneratorName;
            ReminderLevel = NextGeneratorForNotification.ReminderLevel;

            FinalNotificationDate = AllGeneratorSchedules
                                    .Where(x => x.GeneratorName == NextGeneratorForNotification.GeneratorName)
                                    .Where(x => x.IsActive == "Yes")
                                    .Where(x => x.ReminderDateTimeProfile > DateTime.Now)
                                    .OrderBy(x => DateTime.Now - x.ReminderDateTimeProfile)
                                    .LastOrDefault().ReminderDateTimeProfile;
            NextNotificationDuration = NextGeneratorForNotification.ReminderDateTimeProfile - DateTime.Now;

            FirstID = AllGeneratorSchedules
                        .Where(x => x.GeneratorName == NextGeneratorForNotification.GeneratorName)
                        .Where(x => x.IsActive == "Yes")
                        .OrderBy(x => x.Id)
                        .FirstOrDefault().Id;
            LastID = AllGeneratorSchedules
                        .Where(x => x.GeneratorName == NextGeneratorForNotification.GeneratorName)
                        .Where(x => x.IsActive == "Yes")
                        .OrderBy(x => x.Id)
                        .LastOrDefault().Id;

            int SecondsFromNextNotification = (int)(NextNotificationDuration.TotalSeconds);
            //Timer Timer = new Timer(x => NotificationHandlers(), null,
            //                        SecondsFromNextNotification * 1000, Timeout.Infinite);

            Timer Timer = new Timer(new TimerCallback(NotificationHandlers), null,
                                    SecondsFromNextNotification * 1000, Timeout.Infinite);
        }

        public void NotificationHandlers(Object state)
        {
            EmailService emailService = new EmailService();
            emailService.SendMessage(GeneratorName, ReminderLevel, NotificationTime,
                                        NextNotificationDuration, FinalNotificationDate,
                                        FirstID, LastID, GeneratorID);

            Timer.Change(Timeout.Infinite, Timeout.Infinite);
            Timer.Dispose();

            Initialise();
        }    
    }
}
