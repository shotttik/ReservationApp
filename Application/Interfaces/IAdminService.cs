using Application.Common.Requests.Admin;
using Application.Common.Results;
using Domain.Abstractions;
using Domain.DTO;

namespace Application.Interfaces
{
    public interface IAdminService
    {
        Task<Result> UserCreate(UserCreateRequest request);
        Task<Result> UserUpdate(UserUpdateRequest request);
        Task<Result> CompanyCreate(CompanyCreateRequest request);
        Task<Result<PagedList<UserAccountDTO>>> RetrievePagedUsers(PagedParameters parameters, CancellationToken cancellationToken);
    }
}
