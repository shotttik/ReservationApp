using Application.Common.ResultsErrors;

namespace Application.Common.Results.Company
{
    public static class CompanyErrors
    {
        public static readonly Error NotFound = Error.NotFound("Company.NotFound", "Company not found");
        public static readonly Error AlreadyExists = Error.Conflict("Company.AlreadyExists", "Company already exists");
        public static readonly Error InvalidName = Error.Validation("Company.InvalidName", "Company name is invalid");
        public static readonly Error AccessDenied = Error.Forbidden("Company.AccessDenied", "Access to company denied");
    }
}
