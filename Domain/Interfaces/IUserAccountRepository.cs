using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserAccountRepository
    {
        Task<int> AddAsync(UserAccount userAccount);
        Task<UserAccount?> GetAuthorizationData(int ID);
        Task<UserAccount?> GetUserAccountByID(int ID);
        Task Update(UserAccount userAccount);
    }
}
