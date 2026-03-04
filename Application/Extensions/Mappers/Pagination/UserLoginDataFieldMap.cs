namespace Application.Extensions.Mappers.Pagination
{
    public static class UserLoginDataFieldMap
    {
        public static readonly Dictionary<string, string> DtoToEntityPath = new(StringComparer.OrdinalIgnoreCase)
        {

            ["Id"] = "Id",
            ["CompanyId"] = "UserAccount.CompanyId",
            ["FirstName"] = "UserAccount.FirstName",
            ["LastName"] = "UserAccount.LastName",
            ["Email"] = "Email",
            ["VerificationStatus"] = "VerificationStatus",
            ["Role.Name"] = "UserAccount.Role.Name",
            ["ActiveStatus"] = "ActiveStatus",
            ["CreatedAt"] = "CreatedAt",
            ["UpdatedAt"] = "UpdatedAt"
        };
    }
}
