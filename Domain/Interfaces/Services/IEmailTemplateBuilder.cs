using Shared.RabbitMq;

namespace Domain.Interfaces.Services
{
    public interface IEmailTemplateBuilder
    {
        public EmailMessage BuildVerificationEmailMessage(string toEmail, string firstName, string verificationLink);
    }
}
