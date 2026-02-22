namespace Application.Common.Results
{
    public class CompanySubscriptionResults
    {
        #region Errors
        public static readonly Error IsNotActive = Error.Forbidden("CompanySubscription.IsNotActive", "Subscription is not active.");
        public static readonly Error Expired= Error.Forbidden("CompanySubscription.Expired", "Subscription is expired.");
        public static readonly Error NotFound = Error.Forbidden("CompanySubscription.NotFound", "Subscription not found");
        public static readonly Error EmployeeLimitReached = Error.Forbidden("CompanySubscription.EmployeeLimitReached", "Employee limit reached for current plan");
        public static readonly Error BookingLimitReached = Error.Forbidden("CompanySubscription.BookingLimitReached", "Monthly booking limit reached");
        #endregion
        #region Success
        public static readonly SuccessInfo Activated = new("CompanySubscription.Activated", "Branch activated successfully.");
        #endregion
    }
}
