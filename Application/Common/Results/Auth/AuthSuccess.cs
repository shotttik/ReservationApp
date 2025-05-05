using Application.Common.ResultsErrors;

namespace Application.Common.Results.Auth
{
    public static class AuthSuccess
    {
        public static readonly SuccessInfo Login = new("Auth.LoginSuccess", "Successfully logged in");
        public static readonly SuccessInfo Logout = new("Auth.LogoutSuccess", "Successfully logged out");
        public static readonly SuccessInfo EmailVerified = new("Auth.EmailVerified", "Email successfully verified");
        public static readonly SuccessInfo PasswordReset = new("Auth.PasswordReset", "Password successfully reset");
        public static readonly SuccessInfo PasswordChanged = new("Auth.PasswordChanged", "Password successfully changed");
        public static readonly SuccessInfo ProfileUpdated = new("Auth.ProfileUpdated", "Profile successfully updated");
    }
}
