using Domain.DTO.User;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthUser> GetCurrentUser();
        string GetSessionID();
        string GetEmail();
        bool IsInRole(string role);
        int GetUserAccountID();
        int GetUserLoginDataID();
        Task RefreshUserCache(int? userLoginDataID = null);
        Task RefreshAuthUserCache();
        bool IsGuestForBooking(int bookingId);
    }
}
