using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class CompanyRepository :BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
