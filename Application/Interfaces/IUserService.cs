using Application.Common.Requests.User;
using Application.Common.Responses;
using Application.Common.Results;
using Domain.DTO;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<Result<RegisterResponse>> Register(RegisterUserRequest registerUserRequest);
        Task<Result<LoginResponse>> Login(LoginRequest loginRequest);
        Task<Result<RefreshResponse>> Refresh(TokenRequest refreshTokenRequest);
        Task<Result> Logout();
        Task<Result<string>> ForgotPassword(ForgotPasswordRequest request);
        Task<Result> ResetPassword(ResetPasswordRequest request);
        Task<Result<UserAccountDTO>> GetUserAuthorizationDataAsync();
        Task<Result> VerifyEmail(string token);
    }
}
