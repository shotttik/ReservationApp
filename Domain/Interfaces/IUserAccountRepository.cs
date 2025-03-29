using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserAccountRepository
    {
        Task<int> Add(UserAccount userAccount);
        Task<UserAccount?> GetAuthorizationData(int ID);
        Task<UserAccount?> GetUserAccountByID(int ID);
        Task Update(UserAccount userAccount);
    }
}
