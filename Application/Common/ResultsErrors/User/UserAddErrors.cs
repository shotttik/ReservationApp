namespace Application.Common.ResultsErrors.User
{
    public class UserAddErrors
    {
        public static readonly Error AlreadyExists = Error.Conflict("UserAdd.AlreadyExists", "Account with this email already registered.");
        public static readonly Error RoleNotFound = Error.NotFound("UserAdd.RoleNotFound", "User role not found.");
    }
}
