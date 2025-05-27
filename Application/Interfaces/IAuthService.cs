using Domain.DTO.User;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthUser> GetCurrentUser();
        string GetSessionID();
        string GetEmail();
        int GetUserAccountID();
        int GetUserLoginDataID();
        Task RefreshUserCache(int? userLoginDataID = null);
        Task RefreshAuthUserCache();
    }
}
