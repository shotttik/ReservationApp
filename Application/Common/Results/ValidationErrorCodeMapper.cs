namespace Application.Common.Results
{
    public static class ValidationErrorCodeMapper
    {
        private static readonly Dictionary<(string Field, string Message), string> ErrorCodeMap = new()
    {
        { ("Email", "The Email field is not a valid e-mail address."), "Auth.InvalidEmail" },
        { ("Password", "Password must be at least 8 characters long and contain at least one uppercase letter, one number, and one special character."), "Auth.PasswordComplexity" },
        { ("Email", "The Email field is required."), "Auth.EmailRequired" },
        { ("Password", "The Password field is required."), "Auth.PasswordRequired" }
    };

        public static string GetErrorCode(string field, string message)
        {
            return ErrorCodeMap.TryGetValue((field, message), out var code) ? code : "ValidationError.Generic";
        }
    }
}
