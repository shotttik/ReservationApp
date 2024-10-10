namespace Application.Common.ResultsErrors
{
    public static class RefreshTokenErrors
    {
        public static readonly Error SameUser = Error.Conflict("Refresh.SameUser", "User is the same as the one who logged in");
        public static readonly Error NotFound = Error.NotFound("Refresh.NotFound", "User not found");
        public static readonly Error InvalidToken = Error.Validation("Refresh.InvalidToken", "Invalid Refresh Token");
    }
}
