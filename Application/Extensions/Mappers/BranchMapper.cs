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
        public static Branch MapToEntity(this BranchCreateRequest request, Branch entity)
        {
            entity.AddressLine1 = request.AddressLine1;
            entity.AddressLine2 = request.AddressLine2;
            entity.City = request.City;
            entity.PostalCode = request.PostalCode;
            entity.Country = request.Country;
            entity.State = request.State;
            entity.Latitude = request.Latitude;
            entity.Longitude = request.Longitude;

            return entity;
        }
    }
}
