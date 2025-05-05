using Application.Common.ResultsErrors;

namespace Application.Common.Results.Auth
{
    public static class AuthErrors
    {
        public static readonly Error NotFound = Error.NotFound("Auth.UserNotFound", "User not found");
        public static readonly Error InvalidPassword = Error.Validation("Auth.InvalidPassword", "Invalid password");
        public static readonly Error EmailNotVerified = Error.Validation("Auth.EmailNotVerified", "Email is not verified");
        public static readonly Error InvalidToken = Error.Unauthorized("Auth.InvalidToken", "Invalid or expired token");
        public static readonly Error AccountLocked = Error.Forbidden("Auth.AccountLocked", "Account is locked");
    }
}
