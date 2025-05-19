using API.Attributes;
using Application.Authentication;
using Application.Common.Results;
using Application.DTOs.Admin;
using Application.Interfaces;
using Application.Responses;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [Route("api/super-admin")]
    [ApiController]
    public class AdminController :ControllerBase
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [HttpPost("user")]
        [HasPermission(Permission.UserCreate)]
        [Logging(LoggingType.ExceptBody)]
        [ProducesResponseType(typeof(SuccessResponse<RegisterResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UserCreate([FromBody] UserCreateRequest request)
        {
            Result result = await adminService.UserCreate(request);

            return result.IsSuccess ? Ok() : result.ToProblemDetails();
        }

        [HttpPut("user/{userId}")]
        [HasPermission(Permission.UserUpdate)]
        [Logging(LoggingType.Full)]
        public async Task<IActionResult> UserUpdate(int userId, [FromBody] UserUpdateRequest request)
        {
            request.UserAccountID = userId;
            Result result = await adminService.UserUpdate(request);

            return result.IsSuccess ? Ok() : result.ToProblemDetails();
        }

        [HttpPost("company")]
        [HasPermission(Permission.CompanyCreate)]
        [Logging(LoggingType.Full)]
        public async Task<IActionResult> CompanyCreate([FromBody] CompanyCreateRequest request)
        {
            Result result = await adminService.CompanyCreate(request);

            return result.IsSuccess ? Ok() : result.ToProblemDetails();
        }
    }
}
