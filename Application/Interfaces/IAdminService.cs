using Application.Common.Results;
using Application.DTOs.Admin;

namespace Application.Interfaces
{
    public interface IAdminService
    {
        Task<Result> UserCreate(UserCreateRequest request);
        Task<Result> UserUpdate(UserUpdateRequest request);
        Task<Result> CompanyCreate(CompanyCreateRequest request);
    }
}
