using Application.DTOs.User;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserAccountDTO> GetCurrentUser();
    }
}
