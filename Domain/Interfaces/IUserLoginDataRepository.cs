using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserLoginDataRepository :IBaseRepository<UserLoginData>
    {
        Task<UserLoginData?> GetByEmail(string email);
        Task<UserLoginData?> GetFullUserDataByEmail(string email);
        Task<UserLoginData?> GetFullUserData(int ID);
        Task<UserLoginData?> GetByVerificationToken(string verificationToken);
    }
}
