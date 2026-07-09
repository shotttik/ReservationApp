namespace Application.Common.Results
{
    public class AuthResults
    {
        #region Errors
        public static readonly Error NotAuthenticated = Error.Unauthorized("Auth.NotAuthenticated", "User is not authenticated.");
        public static readonly Error UserNotFound = Error.NotFound("Auth.UserNotFound", "User not found");
        public static readonly Error UserDoesntExists = Error.Validation("Auth.UserDoesntExists", "User does not exist");
        public static readonly Error InvalidPassword = Error.Validation("Auth.InvalidPassword", "Invalid password");
        public static readonly Error EmailNotVerified = Error.Validation("Auth.EmailNotVerified", "Email is not verified");

        public static readonly Error InvalidToken = Error.Validation("Auth.InvalidToken", "Invalid token");
        public static readonly Error TokenExpired = Error.Validation("Auth.TokenExpired", "Token expired");

        public static readonly Error AlreadyExists = Error.Conflict("Auth.AlreadyExists", "User already exists");
        public static readonly Error EmailAlreadyExists = Error.Conflict("Auth.EmailAlreadyExists", "Account with this email already registered.");
        public static readonly Error RoleNotFound = Error.Conflict("Auth.RoleNotFound", "Role not found.");
        public static readonly Error RoleIsNotAccessable = Error.Conflict("Auth.RoleIsNotAccessable", "Role is not accessable for registration.");
        public static readonly Error RoleIncompatibility = Error.Conflict("Auth.RoleIncompatibility", "Role compatibility to company is wrong.");

        public static readonly Error ArgumentNull = Error.Conflict("Auth.ArgumentNull", "Nothing to update.");
        public static readonly Error PermissionError = Error.Conflict("Auth.PermissionError", "You don't have permission to update this user.");
        public static readonly Error EmailChangeAlreadyRequested = Error.Conflict("Auth.EmailChangeAlreadyRequested", "You have already requested email change, please wait until token expired.");
        public static readonly Error SessionNotFound = Error.NotFound("Auth.SessionNotFound", "Session not found.");
        public static readonly Error NoActiveSessions = Error.NotFound("Auth.NoActiveSessions", "No active sessions found for this user.");
        public static readonly Error UserAlreadyAssignedToCompany = Error.Conflict("Auth.UserAlreadyAssignedToCompany", "User already assigned to this company with specific role.");
        public static readonly Error InvalidId = Error.Validation("Auth.InvalidId", "Invalid user id provided.");
        public static readonly Error UserDisabledCantBeUsed = Error.Validation("Auth.UserDisabledCantBeUsed", "User is disabled and cannot be used. Please contact support for more information.");
        public static readonly Error UserAlreadyDisabled = Error.Validation("Auth.UserAlreadyDisabled", "User is already disabled.");
        public static readonly Error UserAlreadyActived = Error.Validation("Auth.UserAlreadyActived", "User is already actived.");
        #endregion

        #region Success
        public static readonly SuccessInfo Success = new("Auth.Success", "User authenticated successfully.");
        public static readonly SuccessInfo Registered = new("Auth.Registered", "User registered successfully, Check your email for verification.");
        public static readonly SuccessInfo Logouted = new("Auth.Logouted", "User logouted successfully.");
        public static readonly SuccessInfo CheckEmail = new("Auth.CheckEmail", "Check your email for further instructions.");
        public static readonly SuccessInfo PasswordReseted = new("Auth.PasswordReseted", "Password reseted successfully.");
        public static readonly SuccessInfo EmailVerified = new("Auth.EmailVerified", "Email verified successfully.");
        public static readonly SuccessInfo PasswordChanged = new("Auth.PasswordChanged", "Password changed successfully.");
        public static readonly SuccessInfo UserUpdated = new("Auth.UserUpdated", "User updated successfully.");
        public static readonly SuccessInfo SessionRemoved = new("Auth.SessionRemoved", "Session removed successfully.");
        public static readonly SuccessInfo AllSessionsRemoved = new("Auth.AllSessionsRemoved", "All sessions removed successfully, and logged out.");
        public static readonly SuccessInfo SessionsRemoved = new("Auth.SessionsRemoved", "other active sessions removed successfully.");
        public static readonly SuccessInfo UserCreated = new("Auth.UserCreated", "User created successfully.");
        public static readonly SuccessInfo UserAssignedToCompany = new("Auth.UserAssignedToCompany", "User assigned to company successfully.");

        public static readonly SuccessInfo UserDisabled = new("Auth.UserDisabled", "User disabled successfully.");
        public static readonly SuccessInfo UserDeleted = new("Auth.UserDeleted", "User deleted successfully.");
        public static readonly SuccessInfo UserReactivated = new("Auth.UserReactivated", "User reactivated successfully.");
        #endregion
    }
}
