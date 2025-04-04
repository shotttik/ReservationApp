using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CompanyInvitationRepository :BaseRepository<CompanyInvitation>, ICompanyInvitationRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly DbSet<CompanyInvitation> dbSet;

        public CompanyInvitationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.CompanyInvitations;
        }

        public Task<CompanyInvitation?> Get(string token)
        {
            return dbSet.FirstOrDefaultAsync(x => x.Token == token);
        }
    }
}
