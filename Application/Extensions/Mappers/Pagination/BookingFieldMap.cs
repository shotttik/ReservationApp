namespace Application.Extensions.Mappers.Pagination
{
    public static class BookingFieldMap
    {
        public static readonly Dictionary<string, string> DtoToEntityPath = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Id"] = "Id",
            ["ClientId"] = "ClientId",
            ["EmployeeId"] = "EmployeeId",
            ["CompanyId"] = "CompanyId",
            ["ServiceName"] = "ServiceName",
            ["StartTime"] = "StartTime",
            ["EndTimeExpected"] = "EndTimeExpected",
            ["EndTime"] = "EndTime",
            ["PriceExpected"] = "PriceExpected",
            ["PriceFull"] = "PriceFull",
            ["Discount"] = "Discount",
            ["PriceFinal"] = "PriceFinal",
            ["Status"] = "Status",
            ["CreatedAt"] = "CreatedAt",
            ["UpdatedAt"] = "UpdatedAt"
        };
    }
}
