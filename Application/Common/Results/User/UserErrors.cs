using Application.Common.ResultsErrors;

namespace Application.Common.Results.User
{
    public static class UserErrors
    {
        public static readonly Error NotFound = Error.NotFound("User.NotFound", "User not found");
        public static readonly Error AlreadyExists = Error.Conflict("User.AlreadyExists", "User already exists");
        public static readonly Error InvalidEmail = Error.Validation("User.InvalidEmail", "Invalid email format");
        public static readonly Error WeakPassword = Error.Validation("User.WeakPassword", "Password doesn't meet security requirements");
    }
}
