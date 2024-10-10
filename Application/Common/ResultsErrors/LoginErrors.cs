namespace Application.Common.ResultsErrors
{
    public static class LoginErrors
    {
        public static readonly Error SameUser = Error.Conflict("Login.SameUser", "User is the same as the one who logged in");
        public static readonly Error NotFound = Error.NotFound("Login.NotFound", "User not found");
        public static readonly Error InvalidPassword = Error.Validation("Login.InvalidPassword", "Invalid password");
    }
}
