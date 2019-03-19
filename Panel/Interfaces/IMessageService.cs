using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Panel.Interfaces
{
    public interface IMessageService
    {
        MailAddress FromAddress { get; set; }
        List<MailAddress> ToAddresses { get; set; }
        string MessageNotification { get; set; }
        void SendMessage(string GeneratorName, string ReminderLevel,  
            TimeSpan NextNotificationDuration, DateTime FinalNotificationDate, int FirstID, 
            int LastID, int GeneratorID, string MaintenanceDeliverable);
    }
}
