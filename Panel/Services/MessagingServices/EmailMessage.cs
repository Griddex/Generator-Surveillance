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
                case "Elevated":
                case "Extreme":
                    subject = $@"<b>{ReminderLevel} Reminder:</b> Scheduled Maintenance for <b>{GeneratorName}</b> Generator";
                    body = $@"Dear Team, 
                            {Environment.NewLine}
                            {Environment.NewLine}
                            {reminderCondition} reminder to execute {Environment.NewLine}
                            scheduled maintenance for {GeneratorName} Generator
                            {Environment.NewLine}
                            Scheduled Maintenance Date: <b>{FinalNotificationDate}</b>
                            Time Left: <b>{NextNotificationDuration.Days} days,</b>
                                       {Environment.NewLine}
                                       <b>{NextNotificationDuration.Hours} hours,</b>
                                       {Environment.NewLine}
                                       <b>{NextNotificationDuration.Minutes} minutes,</b>
                                       {Environment.NewLine}
                                       <b>{NextNotificationDuration.Seconds} seconds.</b>
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
