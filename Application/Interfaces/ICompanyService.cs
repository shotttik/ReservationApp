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
        Task<Result> ServicesCreate(int routeCompanyId, ServicesCreateRequest request);
        Task<Result> ServicesUpdate(int routeCompanyId, ServicesUpdateRequest request);
        Task<Result> ServicesDelete(int routeCompanyId, int ID);
        Task<Result<PagedList<CompanyDTO>>> RetrievePaged(
           PagedParameters parameters,
           CancellationToken cancellationToken,
           bool forPublic);
        Task<Result<CompanyDTO>> Get(int id, bool forPublic);
        Task<Result> UploadImages(UploadCompanyImagesRequest request, CancellationToken cancellationToken);
        Task<Result> Update(CompanyPartialUpdateRequest request);
        Task<Result> CreateMember(int routeCompanyId, MemberCreateRequest request);
        Task<Result> UpdateMember(int routeCompanyId, MemberUpdateRequest request);
    }
}
