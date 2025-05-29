namespace Application.Common.Results
{
    public class CompanyResults
    {
        #region Errors
        public static readonly Error AlreadyExists = Error.Conflict("Company.AlreadyExists", "A company with the specified details already exists.");
        public static readonly Error InviteNotFound = Error.NotFound("Company.InviteNotFound", "Invite not found by given token.");
        public static readonly Error InviteTokenExpired = Error.Validation("Company.InviteTokenExpired", "Token is already expired.");
        public static readonly Error InviteInvalidUser = Error.Validation("Company.InviteInvalidUser", "This invite is not for your user.");
        public static readonly Error InviteMemberNotFound = Error.Validation("Company.InviteMemberNotFound", "Member not found for invitation.");
        public static readonly Error InviteInvalidRole = Error.Validation("Company.InviteInvalidRole", "Auth user is not a company admin or invited person is not a user.");
        public static readonly Error ServiceNotFound = Error.NotFound("Company.ServiceNotFound", "Company service not found.");
        public static readonly Error CompanyNotFound = Error.NotFound("Company.CompanyNotFound", "Company not found by given ID.");
        public static readonly Error CompanyDoesNotExists = Error.Validation("Company.CompanyNotExists", "Company does not exist.");
        #endregion
    }
}
