namespace Application.Common.ResultsErrors.User
{
    public class AuthorizationDataErrors
    {
        public static readonly Error NotFound = Error.Forbidden("AuthorizationData.NotFound", "User is not authenticated.");
    }
}
