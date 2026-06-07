using Domain.Entities.CompanyReleated;

namespace Domain.Interfaces.Repositories
{
    public interface IServiceRepository :IBaseRepository<Service>
    {
        Task<IEnumerable<Service>> GetServicesByCompanyId(int companyId, bool forPublic);
        Task<Service?> Get(int id, int companyId);
    }
}
