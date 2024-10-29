namespace Application.Common.ResultsErrors.User
{
    public class UserUpdateErrors
    {
        public static readonly Error ArgumentNull = Error.Conflict("UserUpdate.ArgumentNull", "Nothing to update.");
        public static readonly Error NotFound = Error.Conflict("UserUpdate.NotFound", "User not found.");

    }
}
