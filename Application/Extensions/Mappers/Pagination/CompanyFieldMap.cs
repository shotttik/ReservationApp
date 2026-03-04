namespace Application.Extensions.Mappers.Pagination
{
    public static class CompanyFieldMap
    {
        public static readonly Dictionary<string, string> DtoToEntityPath = new(StringComparer.OrdinalIgnoreCase)
        {

            ["Id"] = "Id",
            ["Name"] = "Name",
            ["Description"] = "Description",
            ["IN"] = "IN",
            ["Email"] = "Email",
            ["Phone"] = "Phone",
            ["Type"] = "Type",
            ["ActiveStatus"] = "ActiveStatus",
            ["CreatedAt"] = "CreatedAt",
            ["UpdatedAt"] = "UpdatedAt",
        };
    }
}
