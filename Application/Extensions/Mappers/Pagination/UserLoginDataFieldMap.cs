namespace Application.Extensions.Mappers.Pagination
{
    public static class UserLoginDataFieldMap
    {
        public static readonly Dictionary<string, string> DtoToEntityPath = new(StringComparer.OrdinalIgnoreCase)
        {

            ["ID"] = "ID",
            ["CompanyID"] = "UserAccount.CompanyID",
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
