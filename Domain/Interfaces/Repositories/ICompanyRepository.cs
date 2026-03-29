using Domain.Abstractions;
using Domain.DTO.Company;
using Domain.Entities.CompanyReleated;

namespace Domain.Interfaces.Repositories
{
    public interface ICompanyRepository :IBaseRepository<Company>
    {
        Task<bool> ExistsByDetailsAsync(string IN, string name, string? email, string? phone, int? excludeId = null);
        Task<Company?> GetFullData(int id);
        Task<Company?> GetFullDataPublic(int id);
        Task<PagedList<CompanyDTO>> RetrievePaged(
            PagedParameters parameters,
            CancellationToken cancellationToken,
            bool forPublic);
        Task<Company?> GetWithBranches(int id);
        Task<Company?> GetWithBranchesAndServices(int id);
        Task<Company?> GetWithMedia(int id);
    }
}
