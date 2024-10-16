namespace Application.Common.ResultsErrors
{
    public static class ForgotPasswordErrors
    {
        public static readonly Error NotFound = Error.NotFound("Login.NotFound", "User not found");
    }
}
