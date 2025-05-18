namespace Application.Common.Results
{
    public static class PagedListResults
    {
        public static Error InvalidPagedParameters(string description) => Error.Validation("PagedList.PagedParametersError", description);
    }
}
