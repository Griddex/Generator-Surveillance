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
        public Timer Timer { get; set; }
        public string GeneratorName { get; set; }
        public string ReminderLevel { get; set; }
        public string NotificationTime { get; set; }

        public Notifier()
        {
            Initialise();
        }

        public void Initialise()
        {
            GeneratorSchedulerRepository gs = new GeneratorSchedulerRepository(new GeneratorSurveillanceDBEntities());
            var CurrentActiveGenerators = gs.GetActiveGeneratorSchedules();
            var NextGeneratorForNotification = CurrentActiveGenerators
                                                .OrderBy(x => x.Starts - DateTime.Now)
                                                .FirstOrDefault();
            int SecondsFromNextNotification = Convert.ToInt32(NextGeneratorForNotification.Starts - DateTime.Now);
            Timer Timer = new Timer(x => NotificationHandlers(), null,
                                SecondsFromNextNotification, Timeout.Infinite);
        }

        public void NotificationHandlers()
        {
            //Call notification method to send email
            EmailService emailService = new EmailService();
            emailService.SendMessage(GeneratorName, ReminderLevel, NotificationTime);

            //Dispose timer
            Timer.Change(Timeout.Infinite, Timeout.Infinite);
            Timer.Dispose();
            //Call Initialise to queue next generator for notification
        }    
    }
}
