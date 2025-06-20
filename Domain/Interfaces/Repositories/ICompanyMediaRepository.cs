using Domain.Entities.CompanyReleated;

namespace Domain.Interfaces.Repositories
{
    public interface ICompanyMediaRepository
    {
        Task<CompanyMedia> Add(CompanyMedia entity, CancellationToken cancellationToken);
        Task<CompanyMedia> Update(CompanyMedia entity);
    }
}
