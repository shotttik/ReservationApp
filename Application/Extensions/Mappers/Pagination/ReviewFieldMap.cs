namespace Application.Extensions.Mappers.Pagination
{
    public static class ReviewFieldMap
    {
        public static Dictionary<string, string> DtoToEntityPath(bool forPublic)
        {
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {

                ["Id"] = "Id",
                ["Overall"] = "Overall",
                // if forpublic is false then add Status map
                ["Locale"] = "Locale",
                ["PublishedAt"] = "PublishedAt",
                ["CreatedAt"] = "CreatedAt",
                ["UpdatedAt"] = "UpdatedAt",
                ["ClientId"] = "ReviewInvite.Booking.ClientID",
                ["EmployeeId"] = "ReviewInvite.Booking.EmployeeID",
                ["CompanyId"] = "ReviewInvite.Booking.CompanyID"
            };

            if (!forPublic)
            {
                map ["Status"] = "Status";
            }

            return map;
        }
    }
}
