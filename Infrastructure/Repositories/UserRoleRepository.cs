using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    class UserRoleRepository :IUserRoleRepository
    {
        private readonly UserDbContext context;

        public UserRoleRepository(UserDbContext context)
        {
            this.context = context;
        }

        public async Task<UserRole?> GetUserRole(int ID)
        {
            var userRole = await context.UserRoles.FindAsync(ID);
            
            return userRole;
        }
    }
}
