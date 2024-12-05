using Application.Common.ResultsErrors;
using Application.DTOs.Admin;

namespace Application.Interfaces
{
    public interface IAdminService
    {
        Task<Result> AddUser(AddUserRequest request);
        Task<Result> UpdateUser(UpdateUserRequest request);
    }
}
