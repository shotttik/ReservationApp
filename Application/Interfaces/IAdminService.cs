using Application.Common.Requests;
using Application.Common.Requests.Admin;
using Application.Common.Results;
using Domain.Abstractions;
using Domain.DTO.User;

namespace Application.Interfaces
{
    public interface IAdminService
    {
        Task<Result> UserCreate(UserCreateRequest request);
        Task<Result> UserUpdate(int id, UserUpdateRequest request);
        Task<Result> ResetUserPassword(int id, AdminResetPasswordRequest request);
        Task<Result> CompanyUpdate(int id, CompanyUpdateRequest request);
        Task<Result> CompanyCreate(CompanyCreateRequest request);
        Task<Result<PagedList<UserLoginDataDTO>>> RetrievePagedUsers(PagedParameters parameters, CancellationToken cancellationToken);
        Task<Result> AssignUserToCompany(AssignUserToCompanyRequest request);
        Task<Result<UserLoginDataDTO>> GetUser(int id);
        Task<Result> ChangeUserActiveStatus(ChangeStatusRequest request, int userId);
    }
}
