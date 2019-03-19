using Panel.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Windows;

namespace Panel.Services.MessagingServices
{
    public class EmailService : IEmailService
    {
        public MailAddress FromAddress { get; set; } =
            new MailAddress("gideonyte@hotmail.com",
                "Generator Surveillance Notification");
        public SmtpClient smtpClient { get; set; } = new SmtpClient();
        public MailMessage mailMessage { get; set; } = new MailMessage();
        public List<MailAddress> ToAddresses { get; set; }
        public List<MailAddress> CCAddresses { get; set; }
        public string MessageNotification { get; set; }
        public Tuple<string, string> SubjectAndBody { get; set; }
        public string LogFilePath { get; set; }

        public EmailService()
        {
            LogFilePath = (string)Application.Current.Properties["LogFile"];

        }

        public void SendMessage(string GeneratorName, string ReminderLevel,
                            TimeSpan NextNotificationDuration, 
                            DateTime FinalNotificationDate, 
                            int FirstID, int LastID, int GeneratorID,
                            string MaintenanceDeliverable)
        {
            try
            {
                mailMessage.From = FromAddress;

                ExtractEmails extractEmails = new ExtractEmails();
                foreach (var email in extractEmails.GetEmails())
                {
                    mailMessage.To.Add(new MailAddress(email));
                }

                SubjectAndBody = EmailMessage.EmailSubjectAndBody
                                                (GeneratorName,
                                                ReminderLevel,
                                                NextNotificationDuration,
                                                FinalNotificationDate,
                                                FirstID, LastID,
                                                GeneratorID,
                                                MaintenanceDeliverable);

                mailMessage.Subject = SubjectAndBody.Item1;
                mailMessage.Body = SubjectAndBody.Item2;
                mailMessage.IsBodyHtml = true;
            }
            catch { }

            try
            {
                smtpClient.Port = 587;             
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Credentials = new NetworkCredential(
                                            "generator.surveillance@gmail.com",
                                            "generator@1");
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(mailMessage);
                Debug.Print($"Mail sent at:  {DateTime.Now}");
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        using (var logfile = new StreamWriter(LogFilePath, true))
                        {
                            logfile.WriteLineAsync($@"[{DateTime.Now}] --> [{GeneratorName}]  
                            Email Delivery failed - retrying in 5 seconds.");
                        }

                        Thread.Sleep(5000);
                        smtpClient.Send(mailMessage);
                    }
                    else if (status == SmtpStatusCode.GeneralFailure)
                    {
                        using (var logfile = new StreamWriter(LogFilePath, true))
                        {
                            logfile.WriteLineAsync($@"[{DateTime.Now}] --> [{GeneratorName}]  
                            SMTP host could not be found...");
                        }
                    }
                    else
                    {
                        using (var logfile = new StreamWriter(LogFilePath, true))
                        {
                            logfile.WriteLineAsync($@"[{DateTime.Now}] --> [{GeneratorName}]  
                            Failed to deliver message to " +
                            $"{ex.InnerExceptions[i].FailedRecipient}");
                        }
                    }
                }
            }
            catch (SmtpException ex)
            {
                SmtpStatusCode status = ex.StatusCode;               
                if (status == SmtpStatusCode.MailboxBusy ||
                    status == SmtpStatusCode.MailboxUnavailable)
                {   
                    using (var logfile = new StreamWriter(LogFilePath, true))
                    {
                        logfile.WriteLineAsync($"[{DateTime.Now}] --> [{GeneratorName}] "  +
                            "Email Delivery failed - retrying in 5 seconds.");
                    }

                    Thread.Sleep(5000);
                    smtpClient.Send(mailMessage);
                }
                else
                {
                    using (var logfile = new StreamWriter(LogFilePath, true))
                    {
                        logfile.WriteLineAsync($@"[{DateTime.Now}] --> [{GeneratorName}]  
                            Failed to deliver message to email address {ex.Message}");
                    };
                }                
            }
            catch (Exception ex)
            {
                using (var logfile = new StreamWriter(LogFilePath, true))
                {
                    logfile.WriteLineAsync($@"[{DateTime.Now}] --> [{GeneratorName}]  
                            Error {ex.Message}");
                };
            }
        }
    }
}
