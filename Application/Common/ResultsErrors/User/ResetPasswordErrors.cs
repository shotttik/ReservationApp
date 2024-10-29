namespace Application.Common.ResultsErrors.User
{
    public class ResetPasswordErrors
    {
        public static readonly Error NotFound = Error.NotFound("ResetPassword.NotFound", "User not found");
        public static readonly Error InvalidToken = Error.Validation("ResetPassword.InvalidToken", "Invalid token");
        public static readonly Error TokenExpired = Error.Validation("ResetPassword.TokenExpired", "Token expired");

    }
}
