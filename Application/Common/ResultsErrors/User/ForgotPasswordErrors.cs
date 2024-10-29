namespace Application.Common.ResultsErrors.User
{
    public static class ForgotPasswordErrors
    {
        public static readonly Error NotFound = Error.NotFound("ForgotPassword.NotFound", "User not found");
    }
}
