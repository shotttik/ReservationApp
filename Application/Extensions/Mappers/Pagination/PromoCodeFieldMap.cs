namespace Application.Extensions.Mappers.Pagination
{
    public static class PromoCodeFieldMap
    {
        public static readonly Dictionary<string, string> DtoToEntityPath = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Id"] = "Id",
            ["Code"] = "Code",
            ["DiscountAmount"] = "DiscountAmount",
            ["DiscountPercent"] = "DiscountPercent",
            ["ValidFrom"] = "ValidFrom",
            ["ValidTo"] = "ValidTo",
            ["MaxUsage"] = "MaxUsage",
            ["UsedCount"] = "UsedCount",
            ["CompanyId"] = "CompanyId",
            ["MinBookingAmount"] = "MinBookingAmount",
            ["ActiveStatus"] = "ActiveStatus",
            ["CreatedAt"] = "CreatedAt",
            ["UpdatedAt"] = "UpdatedAt"
        };
    }
}
