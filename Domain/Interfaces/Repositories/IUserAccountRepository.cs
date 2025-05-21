using Domain.Abstractions;
using Domain.DTO;
using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IUserAccountRepository :IBaseRepository<UserAccount>
    {
        Task<UserAccount?> GetAuthorizationData(int ID);
        Task<PagedList<UserAccountDTO>> RetrievePaged(
            PagedParameters parameters,
            CancellationToken cancellationToken,
            int authUserID);
    }
}
