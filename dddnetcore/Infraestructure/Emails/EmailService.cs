using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using DDDSample1.Domain.Emails;

namespace DDDSample1.Infrastructure.Emails
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _fromEmail;
        private readonly string _password;

        public EmailService()
        {
            this._smtpServer = Environment.GetEnvironmentVariable("Smtp_Server");
            this._smtpPort = int.Parse(Environment.GetEnvironmentVariable("Smtp_Port"));
            this._fromEmail = Environment.GetEnvironmentVariable("Smtp_From_Email");
            this._password = Environment.GetEnvironmentVariable("Smtp_Password");;
        }

        public async Task SendEmailAsync(List<string> to, string subject, string body)
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_fromEmail, _password);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                foreach (var recipient in to){
                    mailMessage.To.Add(recipient);
                }

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
