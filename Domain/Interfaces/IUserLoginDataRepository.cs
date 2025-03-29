using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserLoginDataRepository
    {
        Task Add(UserLoginData userLoginData);
        Task Update(UserLoginData userLoginData);
        Task<UserLoginData?> GetByEmail(string email);
        Task<UserLoginData?> Get(int ID);
        Task<UserLoginData?> GetFullUserDataByEmail(string email);
        Task<UserLoginData?> GetFullUserData(int ID);
        Task UpdateRefreshToken(int ID, string? refreshToken, DateTime refreshTokenExpirationTime);
        Task UpdateRecoveryToken(int ID, string? recoveryToken, DateTime recoveryTokenTime);
        Task UpdateResetPasswordData(int ID, byte [] passwordHash, byte [] passwordSalt);
        Task<UserLoginData?> GetByVerificationToken(string verificationToken);
    }
}
