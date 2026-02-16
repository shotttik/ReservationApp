namespace Application.Common.Results
{
    public class BranchResults
    {
        #region Errors
        public static readonly Error NotFound = Error.NotFound("Branch.NotFound", "Branch not found.");
        public static readonly Error AlreadyDisabled = Error.Validation("Branch.AlreadyDisabled", "Branch is already disabled.");
        public static readonly Error AlreadyActivated = Error.Validation("Branch.AlreadyActivated", "Branch is already activated.");

        #endregion
        #region Success
        public static readonly SuccessInfo Deleted = new("Branch.Deleted", "Branch deleted successfully.");
        public static readonly SuccessInfo Activated = new("Branch.Activated", "Branch activated successfully.");
        #endregion
    }
}
