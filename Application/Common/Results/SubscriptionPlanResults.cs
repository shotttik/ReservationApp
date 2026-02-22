namespace Application.Common.Results
{
    public class SubscriptionPlanResults
    {
        #region Errors
        public static readonly Error NotFound = Error.NotFound("SubscriptionPlan.NotFound", "Subscription not found");
        #endregion
        #region Success
        public static readonly SuccessInfo Activated = new("CompanySubscription.Activated", "Branch activated successfully.");
        #endregion
    }
}
