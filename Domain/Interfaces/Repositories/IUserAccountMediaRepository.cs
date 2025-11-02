using Domain.Entities.User;

namespace Domain.Interfaces.Repositories
{
    public interface IUserAccountMediaRepository
    {
        Task EmptyThenAdd(UserAccountMedia entity);
        Task Empty(int userAccountId);
    }
}
