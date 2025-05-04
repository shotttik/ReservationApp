using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<Role?> GetRole(int ID);
    }
}
