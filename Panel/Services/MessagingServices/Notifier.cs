using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Panel.Repositories;

namespace Panel.Services.MessagingServices
{
    public class Notifier
    {
        public GeneratorScheduler NextGeneratorForNotification { get; set; }
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
            var CurrentActiveGenerators = gs.GetActiveGeneratorSchedules();

            NextGeneratorForNotification = CurrentActiveGenerators
                                           .OrderBy(x => x.Starts - DateTime.Now)
                                           .FirstOrDefault();
            GeneratorID = NextGeneratorForNotification.Id;

            FinalNotificationDate = CurrentActiveGenerators
                                    .OrderBy(x => x.Starts - DateTime.Now)
                                    .LastOrDefault().ReminderDateTimeProfile;
            NextNotificationDuration = NextGeneratorForNotification.Starts - DateTime.Now;

            FirstID = CurrentActiveGenerators
                        .Where(x => x.GeneratorName == NextGeneratorForNotification.GeneratorName)
                        .Where(x => x.IsActive == "Yes")
                        .OrderBy(x => x.Id)
                        .FirstOrDefault().Id;
            LastID = CurrentActiveGenerators
                        .Where(x => x.GeneratorName == NextGeneratorForNotification.GeneratorName)
                        .Where(x => x.IsActive == "Yes")
                        .OrderBy(x => x.Id)
                        .LastOrDefault().Id;

            int SecondsFromNextNotification = (int)(NextNotificationDuration.TotalSeconds);
            Timer Timer = new Timer(x => NotificationHandlers(), null,
                                    SecondsFromNextNotification, Timeout.Infinite);
        }

        public void NotificationHandlers()
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
