using Application.Common.Requests.Admin;
using Domain.DTO.Branch;
using Domain.Entities.BranchReleated;

namespace Application.Extensions.Mappers
{
    public static class BranchMapper
    {
        public static Branch MapToEntity(this BranchCreateRequest request)
        {
            return new Branch
            {
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                PostalCode = request.PostalCode,
                Country = request.Country,
                State = request.State,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };
        }

        public static BranchDTO MapToDTO(this Branch branch)
        {
            return new BranchDTO
            {
                Id = branch.Id,
                AddressLine1 = branch.AddressLine1,
                AddressLine2 = branch.AddressLine2,
                City = branch.City,
                PostalCode = branch.PostalCode,
                Country = branch.Country,
                State = branch.State,
                Latitude = branch.Latitude,
                Longitude = branch.Longitude
            };
        }
        public static Branch MapToEntity(this BranchUpdateRequest request, Branch existing)
        {
            existing.AddressLine1 = request.AddressLine1;
            existing.AddressLine2 = request.AddressLine2;
            existing.City = request.City;
            existing.PostalCode = request.PostalCode;
            existing.Country = request.Country;
            existing.State = request.State;
            existing.Latitude = request.Latitude;
            existing.Longitude = request.Longitude;

            return existing;
        }
        public static Branch MapToEntity(this BranchUpdateRequest request)
        {
            return new Branch
            {
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                State = request.State,
                PostalCode = request.PostalCode,
                Country = request.Country,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };
        }
    }
}
