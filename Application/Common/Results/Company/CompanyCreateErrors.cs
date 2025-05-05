namespace Application.Common.ResultsErrors.Company
{
    public class CompanyCreateErrors
    {
        public static readonly Error AlreadyExists = Error.Conflict("CreateCompany.AlreadyExists", "A company with the specified details already exists.");
    }
}
