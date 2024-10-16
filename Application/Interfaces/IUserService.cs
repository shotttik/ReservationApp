using Application.Common.ResultsErrors;
using Application.DTOs.User;
using Application.Responses;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task RegisterRequest(RegisterUserRequest registerUserRequest);
        Task<Result<LoginResponse>> Login(LoginRequest loginRequest);
        Task<Result<RefreshResponse>> Refresh(TokenRequest refreshTokenRequest);
        Task<Result> Logout(TokenRequest logoutRequest);
        Task<Result<string>> ForgotPassword(ForgotPasswordRequest request);
        Task<Result> ResetPassword(ResetPasswordRequest request);
    }
}
