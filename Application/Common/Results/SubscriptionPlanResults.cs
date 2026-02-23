using Domain.Entities.Common;

namespace Application.Common.Results
{
    public class SubscriptionPlanResults
    {
        #region Errors
        public static readonly Error NotFound = Error.NotFound("SubscriptionPlan.NotFound", "Subscription not found");
        public static readonly Error IsntExists = Error.Validation("SubscriptionPlan.IsnotExists", "Subscription is not exists.");
        #endregion
        #region Success
        public static readonly SuccessInfo Activated = new("CompanySubscription.Activated", "Branch activated successfully.");
        #endregion
    }
}
