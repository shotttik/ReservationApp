using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserAccountRepository
    {
        Task<int> AddAsync(UserAccount userAccount);
    }
}
