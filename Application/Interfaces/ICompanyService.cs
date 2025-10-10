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
        Task<Result<string>> InviteEmployee(int employeeId);
        Task<Result> InviteAccept(string token);
        Task<Result> ServicesCreate(int routeCompanyId, ServicesCreateRequest request);
        Task<Result> ServicesUpdate(int routeCompanyId, ServicesUpdateRequest request);
        Task<Result> ServicesDelete(int routeCompanyId, int ID);
        Task<Result<PagedList<CompanyDTO>>> RetrievePaged(
           PagedParameters parameters,
           CancellationToken cancellationToken,
           bool forPublic);
        Task<Result<CompanyDTO>> Get(int id, bool forPublic);
        Task<Result<List<int>>> UploadMedia(UploadCompanyMediasRequest request, CancellationToken cancellationToken);
        Task<Result> Update(int routeCompanyId, CompanyPartialUpdateRequest request);
        Task<Result> CreateEmployee(int routeCompanyId, EmployeeCreateRequest request);
        Task<Result> UpdateEmployee(int routeCompanyId, EmployeeUpdateRequest request);
        Task<Result> DeleteEmployee(int routeCompanyId, int employeeId, bool force);
        Task<Result<PagedList<UserLoginDataDTO>>> RetrievePagedCompanyEmployees(int routeCompanyId, PagedParameters parameters, CancellationToken cancellationToken);
        Task<Result> UpdateMedia(int routeCompanyId, List<UpdateCompanyMediaRequest> mediaUpdates);
        Task<Result> ChangeActiveStatus(int routeCompanyId, ChangeStatusRequest request);
    }
}
