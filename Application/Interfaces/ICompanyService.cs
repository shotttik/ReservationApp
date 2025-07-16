using Application.Common.Requests;
using Application.Common.Requests.Company;
using Application.Common.Results;
using Domain.Abstractions;
using Domain.DTO.Company;
using Domain.DTO.User;

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
        Task<Result> UploadMedia(int routeCompanyId, UploadCompanyImagesRequest request, CancellationToken cancellationToken);
        Task<Result> Update(int routeCompanyId, CompanyPartialUpdateRequest request);
        Task<Result> CreateMember(int routeCompanyId, MemberCreateRequest request);
        Task<Result> UpdateMember(int routeCompanyId, MemberUpdateRequest request);
        Task<Result> DeleteMember(int routeCompanyId, int memberID, bool force);
        Task<Result<PagedList<UserLoginDataDTO>>> RetrievePagedCompanyMembers(int routeCompanyId, PagedParameters parameters, CancellationToken cancellationToken);
        Task<Result> UpdateMedia(int routeCompanyId, List<UpdateCompanyMediaRequest> mediaUpdates, CancellationToken cancellationToken);
        Task<Result> ChangeActiveStatus(int routeCompanyId, ChangeStatusRequest request);
    }
}
