using Domain.Entities.Common;
using Domain.Entities.User;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Shared.RabbitMq;
using System.Net;
using System.Reflection;

namespace Infrastructure.Templates
{
    public class EmailTemplateBuilder :IEmailTemplateBuilder
    {
        private readonly string _templateFolderPath;
        private readonly ILogger<IEmailTemplateBuilder> _logger;

        public EmailTemplateBuilder(ILogger<EmailTemplateBuilder> logger)
        {
            var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            _templateFolderPath = Path.Combine(assemblyLocation, "Templates");
            _logger = logger;
        }

        private string BuildFromTemplate(string templateName, Dictionary<string, string> placeholders)
        {
            var templatePath = Path.Combine(_templateFolderPath, $"{templateName}.html");

            if (!File.Exists(templatePath))
            {
                _logger.LogError("Email template '{TemplateName}' not found at path '{TemplatePath}'.",
                    templateName,
                    templatePath);

                return string.Empty;
            }

            var html = File.ReadAllText(templatePath);

            foreach (var placeholder in placeholders)
            {
                html = html.Replace(
                    $"{{{{{placeholder.Key}}}}}",
                    WebUtility.HtmlEncode(placeholder.Value));
            }

            return html;
        }

        public EmailMessage BuildVerification(string toEmail, string firstName, string verificationLink)
        {
            const string Subject = "Verify your email";
            var placeholders = new Dictionary<string, string>
            {
                { "FirstName", firstName },
                { "VerificationLink", verificationLink },
                { "Year", DateTime.UtcNow.Year.ToString() }

            };
            var htmlBody = BuildFromTemplate("VerificationEmail", placeholders);
            var emailMessage = new EmailMessage()
            {
                ToEmail = toEmail,
                Subject = Subject,
                Body = htmlBody
            };

            return emailMessage;
        }

        public EmailMessage BuildCodeVerification(
            string toEmail,
            string? firstName,
            string verificationCode,
            int expMinutes,
            Booking booking)
        {
            const string Subject = "Your booking verification code";

            var placeholders = new Dictionary<string, string>
    {
        { "FirstName", string.IsNullOrWhiteSpace(firstName) ? "Guest" : firstName },
        { "VerificationCode", verificationCode },
        { "ExpirationMinutes", expMinutes.ToString() },
        { "Year", DateTime.UtcNow.Year.ToString() },

        // Booking info
        { "BookingReference", booking.Reference },
        { "BookingStatus", booking.Status.ToString() },
        { "BookingDate", booking.StartTime.ToString("dd MMM yyyy") },
        { "BookingStartTime", booking.StartTime.ToString("HH:mm") },
        { "BookingEndTime", booking.EndTimeExpected.ToString("HH:mm") },

        // Service info
        { "ServiceName", booking.Service?.Name ?? "Selected service" },
        { "ServiceDuration", booking.Service?.Duration.ToString() ?? "N/A" },

        // Branch info
        { "BranchName", booking.Branch?.City ?? "Selected City" },
        { "BranchAddress", booking.Branch?.AddressLine1 ?? "Address not specified" },

        // Employee info
        { "EmployeeName", GetEmployeeDisplayName(booking.Employee) },

        // Price info
        { "PriceExpected", booking.PriceExpected.ToString("0.##") },
        { "PriceFinal", (booking.PriceFinal ?? booking.PriceExpected).ToString("0.##") },

        // Optional info
        { "PromoCode", booking.PromoCodeValue ?? "Not applied" },
        { "BookingNote", string.IsNullOrWhiteSpace(booking.Note) ? "No note provided" : booking.Note }
    };

            var htmlBody = BuildFromTemplate("CodeVerification", placeholders);

            return new EmailMessage
            {
                ToEmail = toEmail,
                Subject = Subject,
                Body = htmlBody
            };
        }

        private static string GetEmployeeDisplayName(UserAccount? employee)
        {
            if (employee == null)
                return "Assigned employee";

            var fullName = $"{employee.FirstName} {employee.LastName}".Trim();

            return string.IsNullOrWhiteSpace(fullName)
                ? "Assigned employee"
                : fullName;
        }
    }
}
