using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CompanyRepository :BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsByDetailsAsync(string IN, string name, string? email, string? phone)
        {
            return await _dbSet.AnyAsync(c =>
                c.Name == name
                || c.IN == IN ||
                (email == null || c.Email == email) ||
                (phone == null || c.Phone == phone));
        }
    }
}
