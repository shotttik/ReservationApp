namespace Application.Common.ResultsErrors.User
{
    public class UserUpdateErrors
    {
        public static readonly Error ArgumentNull = Error.Conflict("UserUpdate.ArgumentNull", "Nothing to update.");
        public static readonly Error NotFound = Error.Conflict("UserUpdate.NotFound", "User not found.");
        public static readonly Error RoleNotFound = Error.Conflict("UserUpdate.RoleNotFound", "Role not found.");
        public static readonly Error PermissionError = Error.Conflict("UserUpdate.PermissionError", "You don't have permission to update this user.");
    }
}
