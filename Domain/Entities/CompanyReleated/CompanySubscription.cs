using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Entities.CompanyReleated
{
    public class CompanySubscription :BaseEntity
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;
        public int SubscriptionPlanId { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public SubscriptionStatus Status { get; set; }

        // Billing kovel dghes albat ghamit chairtveba kholme job da shemowmdeba gadaxdebshi kompanias akvs tu ara gadaxdili
        public bool AutoRenew { get; set; }

        public bool IsActive => Status == SubscriptionStatus.Active;
    }
}
