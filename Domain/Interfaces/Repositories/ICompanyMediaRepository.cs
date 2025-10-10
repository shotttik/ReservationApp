using Domain.Entities.CompanyReleated;

namespace Domain.Interfaces.Repositories
{
    public interface ICompanyMediaRepository
    {
        Task<CompanyMedia> Add(CompanyMedia entity, CancellationToken cancellationToken);
        Task<CompanyMedia> Add(CompanyMedia entity);
        Task AddOrUpdate(IEnumerable<CompanyMedia> companyMedias);
        Task<CompanyMedia> Update(CompanyMedia entity, CancellationToken cancellationToken);
        Task UpdateRange(IEnumerable<CompanyMedia> entities, CancellationToken cancellationToken);
        Task Delete(CompanyMedia entity, CancellationToken cancellationToken);
        Task DeleteRange(IEnumerable<CompanyMedia> companyMedia);
    }
}
