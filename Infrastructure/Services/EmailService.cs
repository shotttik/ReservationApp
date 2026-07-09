using Application.Options;
using Domain.Interfaces.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Shared.RabbitMq;
using System.Diagnostics;

namespace Infrastructure.Services
{
    public class EmailService :IEmailService
    {
        private readonly SmtpOptions _smtpSettings;
        private readonly ILogger<IEmailService> _logger;

        public EmailService(
            IOptions<SmtpOptions> smtpSettings,
            ILogger<IEmailService> logger)
        {
            _smtpSettings = smtpSettings.Value;
            _logger = logger;
        }

        public async Task SendEmail(EmailMessage message)
        {
            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(new MailboxAddress(_smtpSettings.Name, _smtpSettings.Username));

            mimeMessage.To.Add(MailboxAddress.Parse(message.ToEmail));

            mimeMessage.Subject = message.Subject;

            mimeMessage.Body = new TextPart("html")
            {
                Text = message.Body
            };

            using var client = new SmtpClient();
            var stopwatch = Stopwatch.StartNew();

            try
            {
                _logger.LogInformation("Attempting to send email: To={ToEmail}, Subject={Subject}", message.ToEmail, message.Subject);
                await client.ConnectAsync(
                    _smtpSettings.Host,
                    _smtpSettings.Port,
                    SecureSocketOptions.SslOnConnect
                    ).ConfigureAwait(false);

                _logger.LogInformation("Connected to SMTP host {Host}:{Port}", _smtpSettings.Host, _smtpSettings.Port);
                await client.AuthenticateAsync(
                    _smtpSettings.Username,
                    _smtpSettings.Password
                    ).ConfigureAwait(false);

                await client.SendAsync(mimeMessage).ConfigureAwait(false);
                stopwatch.Stop();
                _logger.LogInformation("Email successfully sent to {ToEmail} in {Elapsed} ms", message.ToEmail, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex,
                    "Failed to send email to {ToEmail}. Subject={Subject}, Duration={Elapsed} ms",
                    message.ToEmail, message.Subject, stopwatch.ElapsedMilliseconds);
            }
            finally
            {
                if (client.IsConnected)
                {
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                    _logger.LogInformation("Disconnected from SMTP server {Host}", _smtpSettings.Host);
                }
            }
        }
    }
}
