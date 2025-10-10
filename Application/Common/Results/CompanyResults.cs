namespace Application.Common.Results
{
    public class CompanyResults
    {
        #region Errors
        public static readonly Error AlreadyExists = Error.Conflict("Company.AlreadyExists", "A company with the specified details already exists.");
        public static readonly Error InviteNotFound = Error.NotFound("Company.InviteNotFound", "Invite not found by given token.");
        public static readonly Error InviteTokenExpired = Error.Validation("Company.InviteTokenExpired", "Token is already expired.");
        public static readonly Error InviteInvalidUser = Error.Validation("Company.InviteInvalidUser", "This invite is not for your user.");
        public static readonly Error InviteEmployeeNotFound = Error.Validation("Company.InviteEmployeeNotFound", "Employee not found for invitation.");
        public static readonly Error InviteInvalidRole = Error.Validation("Company.InviteInvalidRole", "Auth user is not a company admin or invited person is not a user.");
        public static readonly Error ServiceNotFound = Error.NotFound("Company.ServiceNotFound", "Company service not found.");
        public static readonly Error CompanyNotFound = Error.NotFound("Company.CompanyNotFound", "Company not found by given ID.");
        public static readonly Error CompanyDoesNotExists = Error.Validation("Company.CompanyNotExists", "Company does not exist.");
        public static readonly Error OnlyOneMainMedia = Error.Validation("Company.OnlyOneMainMedia", "There must be exactly one main media for the company.");

        public static readonly Error MaxFAQCategoriesReached = Error.Validation("Company.MaxFAQCategoriesReached", "You have reached the maximum number of FAQ categories for your company.");
        public static readonly Error FAQLimitReached = Error.Validation("Company.FAQLimitReached", "You have reached the maximum number of FAQs for your company.");
        public static readonly Error EmployeeRegistrationNotAllowed = Error.Validation("Company.EmployeeRegistrationNotAllowed", "You are not allowed to register company employees. Only company admins can do that.");
        #endregion

        #region Success
        public static readonly SuccessInfo Updated = new SuccessInfo("Company.Updated", "Company updated successfully.");

        public static readonly SuccessInfo FAQCreated = new SuccessInfo("Company.FAQCreated", "Company FAQ created successfully.");
        public static readonly SuccessInfo FAQDeleted = new SuccessInfo("Company.FAQDeleted", "Company FAQ deleted successfully.");
        public static readonly SuccessInfo FAQUpdated = new SuccessInfo("Company.FAQUpdated", "Company FAQ updated successfully.");

        public static readonly SuccessInfo FAQCategoryCreated = new SuccessInfo("Company.FAQCategoryCreated", "Company FAQ Category created successfully.");
        public static readonly SuccessInfo FAQCategoryDeleted = new SuccessInfo("Company.FAQCategoryDeleted", "Company FAQ Category deleted successfully.");
        public static readonly SuccessInfo FAQCategoryUpdated = new SuccessInfo("Company.FAQCategoryUpdated", "Company FAQ Category updated successfully.");
        #endregion
    }
}
