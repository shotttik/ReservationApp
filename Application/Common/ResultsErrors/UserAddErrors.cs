namespace Application.Common.ResultsErrors
{
    public class UserAddErrors
    {
        public static readonly Error AlreadyExists = Error.Conflict("AlreadyExists", "Account with this email already registered.");
        public static readonly Error RoleNotFound = Error.NotFound("RoleNotFound", "User role not found.");
    }
}
