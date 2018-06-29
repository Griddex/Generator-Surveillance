using Panel.BusinessLogic.AuxilliaryMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Services.MessagingServices
{
    public static class EmailMessage
    {

        public static Tuple<string,string> EmailSubjectAndBody(string GeneratorName, string ReminderLevel, 
                                        string NotificationTime, TimeSpan NextNotificationDuration,
                                        DateTime FinalNotificationDate, int FirstID, int LastID, 
                                        int GeneratorID)
        {
            string subject = "";
            string body = "";
            string reminderCondition = ReminderCondition
                                        .GetReminderCondition(ReminderLevel, FirstID,
                                        LastID, GeneratorID);

            switch (ReminderLevel)
            {
                case "Normal":
                    subject = $@"<b>Normal Reminder:</b> Scheduled Maintenance 
                                for {GeneratorName} Generator";
                    body = $@"Dear Team, 
                            {Environment.NewLine}
                            {Environment.NewLine}
                            {reminderCondition} reminder to execute {Environment.NewLine}
                            scheduled maintenance for {GeneratorName} Generator
                            {Environment.NewLine}
                            Scheduled Maintenance Date: {FinalNotificationDate}
                            Time Left: {NextNotificationDuration.Days} days,
                                       {Environment.NewLine}
                                       {NextNotificationDuration.Hours} hours,
                                       {Environment.NewLine}
                                       {NextNotificationDuration.Minutes} minutes,
                                       {Environment.NewLine}
                                       {NextNotificationDuration.Seconds} seconds.
                            {Environment.NewLine}
                            {Environment.NewLine}
                            Notification Provided By:
                            {Environment.NewLine}
                            Generator Surveillance Software";
                    return new Tuple<string, string>(subject, body);
                default:
                    break;
            }
            return null;
        }
    }
}
