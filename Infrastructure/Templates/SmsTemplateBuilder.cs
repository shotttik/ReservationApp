using Domain.Interfaces.Services;
using Shared.RabbitMq;

namespace Infrastructure.Templates
{
    public class SmsTemplateBuilder :ISmsTemplateBuilder
    {
        public SmsTemplateBuilder()
        {

        }

        public SmsMessage BuildCodeVerification(string toNumber, string verificationCode, int expMinutes)
        {
            var text = $"""
                Your Reservation App verification code is: {verificationCode}. 
                It expires in {expMinutes}
                minutes.Do not share this code with anyone.
                """;

            var smsMessage = new SmsMessage() { ToNumber = toNumber, Text = text };

            return smsMessage;
        }
    }
}
