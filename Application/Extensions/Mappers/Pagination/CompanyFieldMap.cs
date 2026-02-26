namespace Application.Extensions.Mappers.Pagination
{
    public static class CompanyFieldMap
    {
        public static readonly Dictionary<string, string> DtoToEntityPath = new(StringComparer.OrdinalIgnoreCase)
        {

            ["ID"] = "ID",
            ["Name"] = "Name",
            ["Description"] = "Description",
            ["IN"] = "IN",
            ["Email"] = "Email",
            ["Phone"] = "Phone",
            ["Type"] = "Type",
            ["CreatedAt"] = "CreatedAt",
            ["UpdatedAt"] = "UpdatedAt",
        };
    }
}
