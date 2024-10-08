using Application.DTOs.User;
using Application.Responses;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task RegisterRequest(RegisterUserRequest registerUserRequest);
        Task<LoginResponse> Login(LoginRequest loginRequest);
        Task<RefreshResponse> Refresh(TokenRequest refreshTokenRequest);
        Task Logout(TokenRequest logoutRequest);
    }
}
