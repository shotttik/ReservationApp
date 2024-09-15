using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserAccountRepository
    {
        Task AddAsync(UserAccount userAccount);
    }
}
