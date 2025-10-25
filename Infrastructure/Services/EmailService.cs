using Application.Options;
using Domain.Interfaces.Services;
using Infrastructure.EmailTemplates;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Serilog.Context;
using System.Diagnostics;

namespace Infrastructure.Services
{
    public class EmailService :IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly AppUrls _appUrls;
        private readonly EmailTemplateBuilder _emailTemplateBuilder;
        private readonly ILogger<IEmailService> _logger;

        public EmailService(
            IOptions<SmtpSettings> smtpSettings,
            IOptions<AppUrls> appUrls,
            EmailTemplateBuilder emailTemplateBuilder,
            ILogger<IEmailService> logger)
        {
            _smtpSettings = smtpSettings.Value;
            _appUrls = appUrls.Value;
            _emailTemplateBuilder = emailTemplateBuilder;
            _logger = logger;
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
            var stopwatch = Stopwatch.StartNew();

            try
            {
                _logger.LogInformation("Attempting to send email: To={ToEmail}, Subject={Subject}", toEmail, subject);
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

                await client.SendAsync(message).ConfigureAwait(false);
                stopwatch.Stop();
                _logger.LogInformation("Email successfully sent to {ToEmail} in {Elapsed} ms", toEmail, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex,
                    "Failed to send email to {ToEmail}. Subject={Subject}, Duration={Elapsed} ms",
                    toEmail, subject, stopwatch.ElapsedMilliseconds);
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

        public void SendVerificationEmailAsync(string toEmail, string firstName, string verificationToken)
        {
            const string Subject = "Verify your email";
            var verificationLink = $"{_appUrls.ApiBaseUrl}/api/v1/auth/verify-email?token={verificationToken}";
            Task.Run(async () =>
            {
                var correlationId = Guid.NewGuid().ToString();
                using (LogContext.PushProperty("LogTarget", "BackgroundTask"))
                using (LogContext.PushProperty("TaskName", nameof(SendEmail)))
                using (LogContext.PushProperty("CorrelationId", correlationId))
                {
                    _logger.LogInformation("Starting background task to send verification email.");
                    var htmlBody = _emailTemplateBuilder.BuildVerificationEmail(firstName, verificationLink);
                    await SendEmail(toEmail, Subject, htmlBody);
                    _logger.LogInformation("Completed background task to send verification email.");
                }
            });
        }
    }
}
