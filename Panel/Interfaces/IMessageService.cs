using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Interfaces
{
    public interface IMessageService
    {
        MailAddress FromAddress { get; set; }
        List<MailAddress> ToAddresses { get; set; }
        string MessageNotification { get; set; }
        void SendMessage();
    }
}
