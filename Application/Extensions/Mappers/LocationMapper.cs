using Application.Common.Requests.Admin;
using Domain.Entities.LocationReleated;

namespace Application.Extensions.Mappers
{
    public static class LocationMapper
    {
        public static Location MapToEntity(this LocationCreateRequest request)
        {
            return new Location
            {
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                PostalCode = request.PostalCode,
                Country = request.Country
            };
        }
    }
}
