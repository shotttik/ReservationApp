using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    class RoleRepository :IRoleRepository
    {
        private readonly ApplicationDbContext context;

        public RoleRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Role?> GetRole(int ID)
        {
            var userRole = await context.Roles.FindAsync(ID);
 
            return userRole;
        }

    }
}
