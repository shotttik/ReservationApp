using Application.Common.ResultsErrors;

namespace Application.Common.Results.Company
{
    public static class CompanySuccess
    {
        public static readonly SuccessInfo Created = new("Company.Created", "Company successfully created");
        public static readonly SuccessInfo Updated = new("Company.Updated", "Company successfully updated");
        public static readonly SuccessInfo Deleted = new("Company.Deleted", "Company successfully deleted");
        public static readonly SuccessInfo Retrieved = new("Company.Retrieved", "Company successfully retrieved");
    }

}
