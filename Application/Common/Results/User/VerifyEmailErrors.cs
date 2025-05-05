namespace Application.Common.ResultsErrors.User
{
    public class VerifyEmailErrors
    {
        public static readonly Error NotFound = Error.NotFound("VerifyEmailErrors.NotFound", "User data not found.");
        public static readonly Error ExpiredToken = Error.Validation("VerifyEmailErrors.ExpiredToken", "Token expired.");
    }
}
