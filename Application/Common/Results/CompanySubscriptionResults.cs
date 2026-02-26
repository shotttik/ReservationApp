namespace Application.Common.Results
{
    public class CompanySubscriptionResults
    {
        #region Errors
        public static readonly Error IsNotActive = Error.Forbidden("CompanySubscription.IsNotActive", "Subscription is not active.");
        public static readonly Error Expired = Error.Forbidden("CompanySubscription.Expired", "Subscription is expired.");
        public static readonly Error NotFound = Error.Forbidden("CompanySubscription.NotFound", "Subscription not found.");
        public static readonly Error EmployeeLimitReached = Error.Forbidden("CompanySubscription.EmployeeLimitReached", "Employee limit reached for current plan.");
        public static readonly Error BookingLimitReached = Error.Forbidden("CompanySubscription.BookingLimitReached", "Monthly booking limit reached.");
        public static readonly Error IsnotExtendable = Error.Validation("CompanySubscription.IsnotExtendable", "Company plan is not extendable. Reason: must be active or expired.");
        public static readonly Error IsnotAutoRenewable = Error.Validation("CompanySubscription.IsnotAutoRenewable", "Company plan cannot be set auto renewable");
        #endregion
        #region Success
        public static readonly SuccessInfo Activated = new("CompanySubscription.Activated", "Company subscription activated successfully.");
        public static readonly SuccessInfo Canceled = new("CompanySubscription.Canceled", "Company subscription canceled successfully.");
        public static readonly SuccessInfo PlanChanged = new("CompanySubscription.PlanChanged", "Company plan changed successfully.");
        public static readonly SuccessInfo PlanExtended = new("CompanySubscription.PlanExtended", "Company plan extended successfully");
        public static readonly SuccessInfo AutoRenewSet = new("CompanySubscription.AutoRenewSet", "Company plan auto renew setted.");
        public static readonly SuccessInfo PeriodUpdated = new("CompanySubscription.PeriodUpdated", "Company plan period updated.");
        #endregion
    }
}
