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

        public bool IsActive => Status == SubscriptionStatus.Active && EndDate >= DateTime.UtcNow;
        public bool IsExtendable => Status != SubscriptionStatus.Cancelled;
        public void Activate(DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
                throw new InvalidOperationException("Start date is in the future time than the end date.");

            StartDate = startDate;
            EndDate = endDate;
            Status = SubscriptionStatus.Active;
        }
        public void Cancel()
        {
            Status = SubscriptionStatus.Cancelled;
            AutoRenew = false;
        }
        public void Extend(int months)
        {
            var baseDate = EndDate > DateTime.UtcNow
                ? EndDate
                : DateTime.UtcNow;

            EndDate = baseDate.AddMonths(months);
            Status = SubscriptionStatus.Active;
        }
        public void UpdatePeriod(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
