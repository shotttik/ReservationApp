namespace Application.Extensions.Mappers.Pagination
{
    public static class CompanySubscriptionFieldMap
    {
        public static readonly Dictionary<string, string> DtoToEntityPath = new(StringComparer.OrdinalIgnoreCase)
        {
            ["ID"] = "ID",
            ["CompanyId"] = "CompanyId",
            ["SubscriptionPlanId"] = "SubscriptionPlanId",
            ["StartDate"] = "StartDate",
            ["EndDate"] = "EndDate",
            ["Status"] = "Status",
            ["AutoRenew"] = "AutoRenew",
            ["CreatedAt"] = "CreatedAt",
            ["UpdatedAt"] = "UpdatedAt"
        };
    }
}
