using Application.Common.Requests.Company;
using Application.Common.Results;
using Domain.Abstractions;
using Domain.DTO.Company;

namespace Application.Interfaces
{
    public interface ICompanyService
    {
        Task<Result<string>> InviteMember(int memberID);
        Task<Result> InviteAccept(string token);
        Task<Result> ServicesCreate(ServicesCreateRequest request);
        Task<Result> ServicesUpdate(ServicesUpdateRequest request);
        Task<Result> ServicesDelete(int ID);
        Task<Result<PagedList<CompanyDTO>>> RetrievePaged(
           PagedParameters parameters,
           CancellationToken cancellationToken,
           bool forPublic);
        Task<Result<CompanyDTO>> Get(int id, bool forPublic);
    }
}
