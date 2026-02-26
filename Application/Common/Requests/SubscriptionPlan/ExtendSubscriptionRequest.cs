using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.SubscriptionPlan
{
    public class ExtendSubscriptionRequest
    {
        [Range(1, 120)]
        public int AdditionalMonths { get; set; }
    }
}
