using Domain.Entities.User;

namespace Domain.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<Role?> GetRole(int ID);
    }
}
