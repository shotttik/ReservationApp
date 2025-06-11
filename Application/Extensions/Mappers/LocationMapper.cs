using Application.Common.Requests.Admin;
using Domain.DTO.Location;
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
                Country = request.Country,
                 State = request.State
            };
        }

        public static LocationDTO MapToDTO(this Location location)
        {
            return new LocationDTO
            {
                AddressLine1 = location.AddressLine1,
                AddressLine2 = location.AddressLine2,
                City = location.City,
                PostalCode = location.PostalCode,
                Country = location.Country,
                State = location.State
            };
        }
    }
}
