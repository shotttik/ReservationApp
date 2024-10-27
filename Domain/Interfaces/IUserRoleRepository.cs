using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<UserRole?> GetUserRole(int ID);
    }
}
