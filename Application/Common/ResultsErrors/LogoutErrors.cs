﻿namespace Application.Common.ResultsErrors
{
    public static class LogoutErrors
    {
        public static readonly Error SameUser = Error.Conflict("Logout.SameUser", "User is the same as the one who logged in");
        public static readonly Error NotFound = Error.NotFound("Logout.NotFound", "User not found");
        public static readonly Error InvalidToken = Error.Validation("Logout.InvalidToken", "Invalid Refresh Token");

    }
}