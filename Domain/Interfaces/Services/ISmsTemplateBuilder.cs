using Shared.RabbitMq;

namespace Domain.Interfaces.Services
{
    public interface ISmsTemplateBuilder
    {
        public SmsMessage BuildCodeVerification(string toNumber, string verificationCode, int expMinutes);
    }
}
