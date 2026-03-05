namespace Application.Common.Results
{
    public class SubscriptionPlanResults
    {
        #region Errors
        public static readonly Error NotFound = Error.NotFound("SubscriptionPlan.NotFound", "Subscription not found");
        public static readonly Error DoesntExists = Error.Validation("SubscriptionPlan.DoesntExists", "Subscription does not exists.");
        public static readonly Error CantDelete = Error.Validation("SubscriptionPlan.CantDelete", "Subscription plan cannot be deleted, because it is used by companies.");
        #endregion
        #region Success
        public static readonly SuccessInfo Activated = new("CompanySubscription.Activated", "Branch activated successfully.");
        #endregion
    }
}
