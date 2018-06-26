using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Interfaces
{
    public  interface IEmailService : IMessageService
    {
         List<MailAddress> CCAddresses { get; set; }
    }
}
