namespace Application.Common.ResultsErrors.User
{
    public static class LogoutErrors
    {
        public static readonly Error NotFound = Error.Validation("Logout.NotFound", "User not found");
    }
}
