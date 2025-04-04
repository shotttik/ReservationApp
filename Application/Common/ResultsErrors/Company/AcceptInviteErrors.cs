namespace Application.Common.ResultsErrors.Company
{
    internal class AcceptInviteErrors
    {
        public static readonly Error NotFound = Error.NotFound("AcceptInvite.NotFound", "Invite not found by given token.");
        public static readonly Error TokenExpired = Error.Validation("AcceptInvite.TokenExpired", "Token is already expired.");
        public static readonly Error InvalidUser = Error.Validation("AcceptInvite.InvalidUser", "This invite is not for your user.");
    }
}
