using Application.Common.ResultsErrors;
using Application.DTOs.Company;

namespace Application.Interfaces
{
    public interface ICompanyService
    {
        Task<Result<string>> InviteMember(int memberID);
        Task<Result> AcceptInvite(string token);
        Task<Result> CreateServices(CreateServicesRequest request);
        Task<Result> UpdateServices(UpdateServicesRequest request);
        Task<Result> DeleteServices(int ID);
    }
}
