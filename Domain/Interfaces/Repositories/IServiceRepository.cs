using Domain.Entities.CompanyReleated;

namespace Domain.Interfaces.Repositories
{
    public interface IServiceRepository :IBaseRepository<Service>
    {
        Task<IEnumerable<Service>> GetServicesByCompanyId(int companyId, bool forPublic);
    }
}
