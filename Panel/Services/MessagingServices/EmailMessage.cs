using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Services.MessagingServices
{
    public class EmailMessage
    {

        public Tuple<string,string> Email(string GeneratorName, string ReminderLevel, string NotificationTime)
        {
            string subject = "";
            string body = "";
            switch (ReminderLevel)
            {
                case "Normal":

                default:
                    break;
            }
        }
    }
}
