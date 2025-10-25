using Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Infrastructure.EmailTemplates
{
    public class EmailTemplateBuilder
    {
        private readonly string _templateFolderPath;
        private readonly ILogger<IEmailService> _logger;

        public EmailTemplateBuilder(ILogger<IEmailService> logger)
        {
            var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            _templateFolderPath = Path.Combine(assemblyLocation, "EmailTemplates");
            _logger = logger;
        }

        public string BuildFromTemplate(string templateName, Dictionary<string, string> placeholders)
        {
            var templatePath = Path.Combine(_templateFolderPath, $"{templateName}.html");
            if (!File.Exists(templatePath))
            {
                _logger.LogError($"Email template '{templateName}' not found at path '{templatePath}'.");
                return string.Empty;
            }
            var html = File.ReadAllText(templatePath);
            foreach (var placeholder in placeholders)
            {
                html = html.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
            }

            return html;
        }

        public string BuildVerificationEmail(string firstName, string verificationLink)
        {
            var placeholders = new Dictionary<string, string>
            {
                { "FirstName", firstName },
                { "VerificationLink", verificationLink },
                { "Year", DateTime.UtcNow.Year.ToString() }

            };
            return BuildFromTemplate("VerificationEmail", placeholders);
        }

    }
}
