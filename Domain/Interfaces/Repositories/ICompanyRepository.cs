using Domain.Abstractions;
using Domain.DTO.Company;
using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface ICompanyRepository :IBaseRepository<Company>
    {
        Task<bool> ExistsByDetailsAsync(string IN, string name, string? email, string? phone);
        Task<Company?> GetFullData(int id);
        Task<PagedList<CompanyDTO>> RetrievePaged(
            PagedParameters parameters,
            CancellationToken cancellationToken,
            bool forPublic);
    }
}
