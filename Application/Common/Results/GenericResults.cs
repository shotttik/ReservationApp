namespace Application.Common.Results
{
    public class GenericResults
    {
        #region Errors  
        public static readonly Error NotFound = Error.NotFound("Generic.NotFound", "Resource not found.");
        #endregion
        #region Success
        public static readonly SuccessInfo Success = new("Generic.Success", "Operation completed successfully.");
        public static readonly SuccessInfo Created = new("Generic.Created", "Resource created successfully.");
        public static readonly SuccessInfo Updated = new("Generic.Updated", "Resource updated successfully.");
        public static readonly SuccessInfo Deleted = new("Generic.Deleted", "Resource deleted successfully.");
        #endregion
    }
}
