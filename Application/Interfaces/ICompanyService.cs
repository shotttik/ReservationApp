using Application.Common.ResultsErrors;
using Application.DTOs.Company;

namespace Application.Interfaces
{
    public interface ICompanyService
    {
        Task<Result<string>> InviteMember(int memberID);
        Task<Result> InviteAccept(string token);
        Task<Result> ServicesCreate(ServicesCreateRequest request);
        Task<Result> ServicesUpdate(ServicesUpdateRequest request);
        Task<Result> ServicesDelete(int ID);
    }
}
