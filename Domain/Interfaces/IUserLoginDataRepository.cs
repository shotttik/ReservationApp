using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserLoginDataRepository
    {
        Task AddAsync(UserLoginData userLoginData);
        Task<UserLoginData?> GetByEmailAsync(string email);
        Task UpdateRefreshToken(int ID, string? refreshToken, DateTime refreshTokenExpirationTime);
        Task UpdateRecoveryToken(int ID, string? recoveryToken, DateTime recoveryTokenTime);
        Task UpdateResetPasswordData(int ID, byte [] passwordHash, byte [] passwordSalt);
    }
}
