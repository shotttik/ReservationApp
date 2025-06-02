using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories
{
    public class CompanyInvitationRepository :BaseRepository<CompanyInvitation>, ICompanyInvitationRepository
    {
        private readonly DbSet<CompanyInvitation> dbSet;

        public CompanyInvitationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbSet = dbContext.CompanyInvitations;
        }

        public Task<CompanyInvitation?> Get(string token)
        {
            return dbSet.FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task RevokePreviousInvite(int userAccountID)
        {
            var invitations = await dbSet.Where(e => e.UserAccountID == userAccountID).ToArrayAsync();
            if (invitations.IsNullOrEmpty())
                return;
            foreach (var invitation in invitations)
            {
                invitation.Token = null;
                invitation.ExpirationTime = null;
                invitation.UpdateTimestamp();
            }
            dbSet.UpdateRange(invitations);
            await dbContext.SaveChangesAsync();
        }
    }
}
