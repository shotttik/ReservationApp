namespace Application.Common.ResultsErrors.Company
{
    internal class InviteMemberErrors
    {
        public static readonly Error NotFound = Error.Validation("InviteMember.NotFound", "Member not found for invitation.");
        public static readonly Error NotValidRole = Error.Validation("InviteMember.NotValidRole", "Auth user is not a company admin or invited person is not a user.");
    }
}
