using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IUserAccountRepository :IBaseRepository<UserAccount>
    {
        Task<UserAccount?> GetAuthorizationData(int ID);
    }
}
