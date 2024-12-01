using Application.Common.ResultsErrors;
using Application.DTOs.User;

namespace Application.Interfaces
{
    public interface IAdminService
    {
        Task<Result> AddUser(AddRequest request);
        Task<Result> UpdateUser(UpdateRequest request);
    }
}
