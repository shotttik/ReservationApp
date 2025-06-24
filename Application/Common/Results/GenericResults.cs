namespace Application.Common.Results
{
    public class GenericResults
    {
        #region Errors  
        public static readonly Error NotFound = Error.NotFound("Generic.NotFound", "Resource not found.");
        public static readonly Error AlreadyExists = Error.Conflict("Generic.AlreadyExists", "Resource already exists.");
        public static readonly Error InvalidInput = Error.Validation("Generic.InvalidInput", "Invalid input provided.");
        public static readonly Error EmptyList = Error.Validation("Generic.EmptyList", "The provided list cannot be empty.");
        public static readonly Error InvalidID = Error.Validation("Generic.InvalidID", "ID must be greater than 0.");
        public static readonly Error DontExists = Error.Validation("Generic.DontExists", "Resource do not exists.");
        public static readonly Error IDMismatch = Error.Validation("Generic.IDMismatch", "Route ID and request ID must match.");
        public static readonly Error Forbidden = Error.Forbidden("Generic.Forbidden", "You do not have permission to perform this action.");
        #endregion
        #region Success
        public static readonly SuccessInfo Success = new("Generic.Success", "Operation completed successfully.");
        public static readonly SuccessInfo Created = new("Generic.Created", "Resource created successfully.");
        public static readonly SuccessInfo Updated = new("Generic.Updated", "Resource updated successfully.");
        public static readonly SuccessInfo Deleted = new("Generic.Deleted", "Resource deleted successfully.");
        #endregion
    }
}
