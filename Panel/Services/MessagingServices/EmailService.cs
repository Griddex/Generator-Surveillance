using Panel.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public void SendMessage(string GeneratorName, string ReminderLevel,
            TimeSpan NextNotificationDuration,
            DateTime FinalNotificationDate, int FirstID, int LastID,
            int GeneratorID)
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
                                                FirstID,
                                                LastID, GeneratorID);

                mailMessage.Subject = SubjectAndBody.Item1;
                mailMessage.Body = SubjectAndBody.Item2;
                mailMessage.IsBodyHtml = true;
            }
            catch { }

            try
            {

                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;

                //smtpClient.Host = "smtp.aol.com";
                //smtpClient.Credentials = new NetworkCredential(
                //                            "gideon.sanni@aol.com",
                //                            "KrygySTan#1");

                smtpClient.Host = "smtp.live.com";
                smtpClient.Credentials = new NetworkCredential(
                                            "gideonyte@hotmail.com",
                                            "KazakhSTan#1");

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtpClient.Send(mailMessage);
                Debug.Print($"Mail sent at:  { DateTime.Now}");

            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        MessageBox.Show($"Delivery failed - retrying" +
                            $" in 5 seconds.",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);

                        Thread.Sleep(5000);
                        smtpClient.Send(mailMessage);
                    }
                    else
                    {
                        MessageBox.Show($"Failed to deliver message to " +
                            $"{ex.InnerExceptions[i].FailedRecipient}",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
            catch (SmtpException ex)
            {
                SmtpStatusCode status = ex.StatusCode;
                for (int i = 0; i < 5; i++)
                {
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        MessageBox.Show($"Delivery failed - retrying" +
                            $" in 5 seconds.",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);

                        Thread.Sleep(5000);
                        smtpClient.Send(mailMessage);
                    }
                    else
                    {
                        MessageBox.Show($"Failed to deliver message to ",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
