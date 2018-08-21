using System.Collections.Generic;
using System.Net.Mail;

namespace Panel.Interfaces
{
    public  interface IEmailService : IMessageService
    {
         List<MailAddress> CCAddresses { get; set; }
    }
}
