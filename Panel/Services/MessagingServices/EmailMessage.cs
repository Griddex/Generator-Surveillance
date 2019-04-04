using Panel.BusinessLogic.AuxilliaryMethods;
using System;

namespace Panel.Services.MessagingServices
{
    public static class EmailMessage
    {

        public static Tuple<string,string> EmailSubjectAndBody(string GeneratorName, string ReminderLevel, 
                                        TimeSpan NextNotificationDuration, DateTime FinalNotificationDate, 
                                        int FirstID, int LastID, int GeneratorID, string MaintenanceDeliverable)
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
                    subject = $"{ReminderLevel} Reminder: Scheduled Maintenance for {GeneratorName}  Generator";
                    body = $@"Dear Team, 
                            <br/>
                            <br/>
                            <b>{reminderCondition} reminder</b> for scheduled maintenance of <b>{GeneratorName}</b> Generator.
                            <br/>
                            <br/>
                            <table border='1' style='width: 100 %' bordercolor='#000000' cellspacing='0' cellpadding='10'>
                                <tr align='center'>
                                    <th bgcolor='#99e6ff'>Generator Name</th>
                                    <th bgcolor='#99e6ff'>Maintenance Deliverable</th>
                                    <th bgcolor='#99e6ff'>Maintenance Date & Time Deadline</th>
                                    <th bgcolor='#99e6ff'>Time Left to Maintenance</th>
                                </tr>
                                <tr align='center'>
                                    <td>
                                        <b>{GeneratorName}</b>
                                    </td>
                                    <td>
                                        <b>{MaintenanceDeliverable}</b>
                                    </td>
                                    <td>
                                        <b>{FinalNotificationDate: dddd, MMMM dd, yyyy}</b>
                                        <br/>
                                        <br/>
                                        <b>{FinalNotificationDate: h:mm:ss tt}</b>
                                    </td>
                                    <td>
                                        <b>{NextNotificationDuration.Days}</b> day(s),
                                        <br/>
                                        <b>{NextNotificationDuration.Hours}</b> hour(s),
                                        <br/>
                                        <b>{NextNotificationDuration.Minutes}</b> minute(s),
                                        <br/>
                                        <b>{NextNotificationDuration.Seconds}</b> second(s).
                                    </td>
                                </tr>                                
                            </table>
                            <br/>
                            <br/>
                            Automatic notification by:
                            <br/>
                            Generator Surveillance Software &#174;";
                    return new Tuple<string, string>(subject, body);
                default:
                    break;
            }
            return null;
        }
    }
}
