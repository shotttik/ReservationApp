using Domain.Abstractions;
using Domain.DTO.User;
using Domain.Entities.User;

namespace Domain.Interfaces.Repositories
{
    public interface IUserLoginDataRepository :IBaseRepository<UserLoginData>
    {
        Task<UserLoginData?> GetByEmail(string email);
        Task<UserLoginData?> GetFullUserDataByEmail(string email);
        Task<UserLoginData?> GetFullUserData(int ID);
        Task<UserLoginData?> GetWithUserAccount(int ID, int companyID);
        Task<UserLoginData?> GetByVerificationToken(string verificationToken);
        Task<UserLoginData?> GetByUserAccountID(int userAccountID);
        Task<UserLoginData?> GetByRecoveryToken(string recoveryToken);
        Task<PagedList<UserLoginDataDTO>> RetrievePaged(
           PagedParameters parameters,
           CancellationToken cancellationToken,
           int authUserID);
    }
}
