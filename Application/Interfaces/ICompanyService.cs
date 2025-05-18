using Application.Common.Results;
using Application.DTOs.Company;
using Domain.Abstractions;
using Domain.DTO;

namespace Application.Interfaces
{
    public interface ICompanyService
    {
        Task<Result<string>> InviteMember(int memberID);
        Task<Result> InviteAccept(string token);
        Task<Result> ServicesCreate(ServicesCreateRequest request);
        Task<Result> ServicesUpdate(ServicesUpdateRequest request);
        Task<Result> ServicesDelete(int ID);
        Task<Result<PagedList<CompanyDTO>>> GetPaged(
           PagedParameters parameters,
           CancellationToken cancellationToken);
    }
}
