using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface ICompanyRepository :IBaseRepository<Company>
    {
        Task<bool> ExistsByDetailsAsync(string IN, string name, string? email, string? phone);
    }
}
