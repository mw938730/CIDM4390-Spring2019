using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace IdentityDemo.Areas.Identity.Services
{
    public class EmailSender : IEmailSender
    {

        private string SendGridApiKey { get; }
        public string FromAddress { get; }
        public string FromAddressAlias { get; }

        public EmailSender()
        {
            SendGridApiKey   = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            FromAddress      = Environment.GetEnvironmentVariable("SENDGRID_FROM_ADDRESS");
            FromAddressAlias = Environment.GetEnvironmentVariable("SENDGRID_FROM_ADDRESS_ALIAS");
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(SendGridApiKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(FromAddress, FromAddressAlias),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}