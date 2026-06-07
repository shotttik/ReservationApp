using Domain.Entities.Common;

namespace Application.Common.Results
{
    public class PromoResults
    {
        #region Errors
        public static readonly Error AlreadyDisabled = Error.Validation("PromoCode.AlreadyDisabled", "Promo code is already disabled.");
        public static readonly Error AlreadyActivated = Error.Validation("PromoCode.AlreadyActivated", "Promo code is already activated.");
        public static readonly Error CompanyIsDisabled = Error.Validation("PromoCode.CompanyIsDisabled", "Cant create promo code for disabled company.");
        public static readonly Error CodeAlreadyExists = Error.Validation("PromoCode.AlreadyExists", "This code already exists, and can't created multiple times, delete and create again.");
        public static readonly Error NotFound = Error.NotFound("PromoCode.NotFound", "Promo code not found with given id.");
        #endregion
        #region Success
        public static readonly SuccessInfo Deleted = new("PromoCode.Deleted", "Promo code deleted successfully.");
        public static readonly SuccessInfo Activated = new("BrPromoCodeanch.Activated", "Promo code activated successfully.");
        #endregion

        public static PromoResult Invalid()
        {
            return new PromoResult() { IsValid = false, Error = Error.Validation("PromoCode.Invalid", "Promo code is invalid.") };
        }
        public static PromoResult ServiceNotFound()
        {
            return new PromoResult() { IsValid = false, Error = Error.Validation("PromoCode.ServiceNotFound", "Service for company not found or disabled..") };
        }
        public static PromoResult Expired()
        {
            return new PromoResult() { IsValid = false, Error = Error.Validation("PromoCode.Expired", "Promo code expired.") };
        }
        public static PromoResult LimitReached()
        {
            return new PromoResult() { IsValid = false, Error = Error.Validation("PromoCode.LimitReached", "Promo usage limit reached.") };
        }
        public static PromoResult MinAmountReached()
        {
            return new PromoResult() { IsValid = false, Error = Error.Validation("PromoCode.MinAmountReached", "Minimum amount not reached.") };
        }
    }
    public class PromoResult
    {
        public bool IsValid { get; set; }
        public Error Error { get; set; } = Error.None;

        public decimal Discount { get; set; }
        public PromoCode? Promo { get; set; }
    }
}
