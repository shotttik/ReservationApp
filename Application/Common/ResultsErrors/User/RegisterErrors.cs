﻿namespace Application.Common.ResultsErrors.User
{
    public static class RegisterErrors
    {
        public static readonly Error AlreadyExists = Error.Conflict("Register.AlreadyExists", "Account with this email already registered.");
        public static readonly Error RoleNotFound = Error.Conflict("Register.RoleNotFound", "User role not found.");
    }
}
