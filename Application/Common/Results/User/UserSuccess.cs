using Application.Common.ResultsErrors;

namespace Application.Common.Results.User
{
    public static class UserSuccess
    {
        public static readonly SuccessInfo Created = new("User.Created", "User successfully created");
        public static readonly SuccessInfo Updated = new("User.Updated", "User successfully updated");
        public static readonly SuccessInfo Deleted = new("User.Deleted", "User successfully deleted");
        public static readonly SuccessInfo Retrieved = new("User.Retrieved", "User successfully retrieved");
        public static readonly SuccessInfo RoleAssigned = new("User.RoleAssigned", "Role successfully assigned to user");
    }

}
