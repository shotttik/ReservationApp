namespace Application.Common.Results
{
    public class UserResults
    {
        #region Errors
        // user doesnt exists
        public static readonly Error DoesntExists = Error.Validation("User.DoesntExists", "User doesn't exists.");
        #endregion
    }
}
