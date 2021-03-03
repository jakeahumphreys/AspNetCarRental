using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Web;
using Microsoft.Owin.Security.Facebook;

namespace EIRLSSAssignment1.Common
{
    

    public class EmailHelper
    {
        private readonly Library _library;

        public EmailHelper()
        {
            _library = new Library();
        }

        public SmtpClient ConfigureSmtpClient()
        {
            var config = _library.GetActiveConfiguration();

            var smtpClient = new SmtpClient(config.SmtpUrl)
            {
                Port = config.SmtpPort,
                Credentials = new NetworkCredential(config.SmtpSenderUsername, config.SmtpSenderPassword),
                EnableSsl = config.SmtpShouldUseSsl
            };

            return smtpClient;
        }

        public void SendEmail(string message)
        {
            var smtpClient = ConfigureSmtpClient();
            var config = _library.GetActiveConfiguration();

            if (config.SmtpShouldSendEmail)
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(config.SmtpEmailFrom),
                    Subject = "Assignment 2 DVLA Validation",
                    Body = message,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(config.SmtpRecipientEmail);
                smtpClient.Send(mailMessage);
            }
        }
    }
}