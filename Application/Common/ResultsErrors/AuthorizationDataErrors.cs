namespace Application.Common.ResultsErrors
{
    public class AuthorizationDataErrors
    {
        public static readonly Error NotFound = Error.NotFound("AuthorizationData.NotFound", "User not found");
    }
}
