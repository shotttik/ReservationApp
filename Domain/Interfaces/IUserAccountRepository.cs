using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserAccountRepository :IBaseRepository<UserAccount>
    {
        Task<UserAccount?> GetAuthorizationData(int ID);
    }
}
