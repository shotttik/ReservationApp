using Domain.DTO;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserAccountDTO> GetCurrentUser();
    }
}
