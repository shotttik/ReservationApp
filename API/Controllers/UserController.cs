using API.Attributes;
using Application.Common.ResultsErrors;
using Application.DTOs.User;
using Application.Interfaces;
using Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("API/[controller]/[action]")]
    [ApiController]
    public class UserController :ControllerBase
    {
        private readonly IUserService userAccountService;

        public UserController(IUserService userAccountService)
        {
            this.userAccountService = userAccountService;
        }

        [HttpPost]
        [Logging(LoggingType.ExceptBody)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest registerUserRequest)
        {
            await userAccountService.RegisterRequest(registerUserRequest);

            return Ok();
        }

        [HttpPost]
        [Logging(LoggingType.ExceptBody)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            Result<LoginResponse> result = await userAccountService.Login(loginRequest);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblemDetails();
        }

        [HttpPost]
        [Logging(LoggingType.Full)]
        public async Task<IActionResult> Refresh([FromBody] TokenRequest refreshTokenRequest)
        {
            Result<RefreshResponse> result = await userAccountService.Refresh(refreshTokenRequest);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblemDetails();
        }

        [HttpPost]
        [Logging(LoggingType.Full)]
        public async Task<IActionResult> Logout([FromBody] TokenRequest logoutRequest)
        {
            var result = await userAccountService.Logout(logoutRequest);

            return result.IsSuccess ? Ok() : result.ToProblemDetails();
        }

        [HttpPost]
        [Logging(LoggingType.Full)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest)
        {
            Result<string> result = await userAccountService.ForgotPassword(forgotPasswordRequest);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblemDetails();
        }

        [HttpPost]
        [Logging(LoggingType.ExceptBody)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            Result result = await userAccountService.ResetPassword(resetPasswordRequest);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
    }
}
