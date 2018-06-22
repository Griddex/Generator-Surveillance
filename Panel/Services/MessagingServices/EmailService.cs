using Panel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Services.MessagingServices
{
    public class EmailService : IEmailService
    {
        public MailAddress FromAddress { get; set; } = new MailAddress("gideonyte@hotmail.com", "Generator Surveillance Notification");
        public List<MailAddress> ToAddresses { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<MailAddress> CCAddresses { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string MessageNotification { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void SendMessage()
        {
            throw new NotImplementedException();
        }
    }
}
