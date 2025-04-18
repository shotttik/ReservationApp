﻿using Application.Common.ResultsErrors;
using Application.DTOs.User;
using Application.Responses;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<Result> Register(RegisterUserRequest registerUserRequest);
        Task<Result<LoginResponse>> Login(LoginRequest loginRequest);
        Task<Result<RefreshResponse>> Refresh(TokenRequest refreshTokenRequest);
        Task<Result> Logout();
        Task<Result<string>> ForgotPassword(ForgotPasswordRequest request);
        Task<Result> ResetPassword(ResetPasswordRequest request);
        Task<Result<UserAccountDTO>> GetUserAuthorizationDataAsync();
        Task<Result> VerifyEmail(string token);
    }
}
