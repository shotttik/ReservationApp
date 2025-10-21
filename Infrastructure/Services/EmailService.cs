using Application.Options;
using Domain.Interfaces.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Services
{
    public class EmailService :IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmail(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_smtpSettings.Name, _smtpSettings.Username));

            message.To.Add(MailboxAddress.Parse(toEmail));

            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(
                    _smtpSettings.Host,
                    _smtpSettings.Port,
                    SecureSocketOptions.SslOnConnect
                    ).ConfigureAwait(false);

                await client.AuthenticateAsync(
                    _smtpSettings.Username, 
                    _smtpSettings.Password
                    ).ConfigureAwait(false);

                await client.SendAsync(message).ConfigureAwait(false);
            }
            finally
            {
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }

    }
}
