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
        Task<Result<string>> InviteEmployee(InviteEmployeeRequest request);
        Task<Result> InviteAccept(string token);
        Task<Result> CreateServices(int routeCompanyId, ServicesCreateRequest request);
        Task<Result> UpdateServices(int routeCompanyId, ServicesUpdateRequest request);
        Task<Result> DeleteServices(int routeCompanyId, int ID);
        Task<Result<List<ServiceDTO>>> RetrieveServices(int routeCompanyId, bool forPublic);
        Task<Result<PagedList<CompanyDTO>>> RetrievePaged(
           PagedParameters parameters,
           CancellationToken cancellationToken);
        Task<Result<CompanyDTO>> Get(int id);
        Task<Result<List<string>>> UploadMedia(int routeCompanyId, UploadCompanyMediaRequest request, CancellationToken cancellationToken);
        Task<Result> Update(int routeCompanyId, CompanyPartialUpdateRequest request);
        Task<Result> CreateEmployee(int routeCompanyId, EmployeeCreateRequest request);
        Task<Result> UpdateEmployee(int routeCompanyId, EmployeeUpdateRequest request);
        Task<Result> DeleteEmployee(int routeCompanyId, int employeeId, bool force);
        Task<Result<PagedList<UserLoginDataDTO>>> RetrievePagedCompanyEmployees(int routeCompanyId, PagedParameters parameters, CancellationToken cancellationToken);
        Task<Result> UpdateMedia(int routeCompanyId, List<UpdateCompanyMediaRequest> mediaUpdates);
        Task<Result> ChangeActiveStatus(int routeCompanyId, ChangeStatusRequest request);
    }
}
