namespace Application.Common.ResultsErrors.User
{
    public static class RefreshTokenErrors
    {
        public static readonly Error NotFound = Error.NotFound("Refresh.NotFound", "User not found");
        public static readonly Error InvalidToken = Error.Validation("Refresh.InvalidToken", "Invalid Refresh Token");
    }
}
