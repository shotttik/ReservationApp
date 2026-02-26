using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.SubscriptionPlan
{
    public class SetAutoRenewRequest
    {
        [Required]
        public bool AutoRenew { get; set; }
    }
}
