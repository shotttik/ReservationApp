using Application.Common.Requests.User;
using Application.Common.Responses;
using Application.Common.Results;
using Domain.DTO;
using Domain.DTO.User;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<Result<RegisterResponse>> Register(RegisterUserRequest registerUserRequest);
        Task<Result<LoginResponse>> Login(LoginRequest loginRequest);
        Task<Result<RefreshResponse>> Refresh(RefreshTokenRequest refreshTokenRequest);
        Task<Result> Logout();
        Task<Result<string>> ForgotPassword(ForgotPasswordRequest request);
        Task<Result> ResetPassword(ResetPasswordRequest request);
        Task<Result<AuthUser>> GetUserAuthorizationDataAsync();
        Task<Result> VerifyEmail(string token);
        Task<Result<RegisterResponse>> ChangeEmail(ChangeEmailRequest request);
        Task<Result> ChangePassword(ChangePasswordRequest request);
        Task<Result> Update(UpdateUserRequest request);
        Task<Result<List<SessionInfoSummaryDTO>>> GetActiveSessions();
        Task<Result> DeleteActiveSession(string sessionId);
        Task<Result> DeleteAllActiveSessions(int? UserID = null);
    }
}
