﻿namespace Application.Common.ResultsErrors.User
{
    public static class LoginErrors
    {
        public static readonly Error NotFound = Error.NotFound("Login.NotFound", "User not found");
        public static readonly Error InvalidPassword = Error.Validation("Login.InvalidPassword", "Invalid password");
        public static readonly Error EmailNotVerified = Error.Validation("Login.EmailNotVerified", "Email is not verified");
    }
}
