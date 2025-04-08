namespace Application.Common.ResultsErrors.User
{
    public static class LogoutErrors
    {
        public static readonly Error SameUser = Error.Conflict("Logout.SameUser", "User is the same as the one who logged in");
        public static readonly Error NotFound = Error.Validation("Logout.NotFound", "User not found");
        public static readonly Error InvalidRefreshToken = Error.Validation("Logout.InvalidToken", "Invalid Refresh Token");
        public static readonly Error InvalidToken = Error.Validation("Logout.InvalidToken", "Invalid Token");
    }
}
