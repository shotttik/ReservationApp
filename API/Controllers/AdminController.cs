using API.Attributes;
using Application.Authentication;
using Application.Common.ResultsErrors;
using Application.DTOs.Admin;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController :ControllerBase
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [HttpPost("users")]
        [HasPermission(Permission.AddUser)]
        [Logging(LoggingType.ExceptBody)]
        public async Task<IActionResult> AddUser([FromBody] AddUserRequest request)
        {
            Result result = await adminService.AddUser(request);

            return result.IsSuccess ? Ok() : result.ToProblemDetails();
        }

        [HttpPut("users/{userId}")]
        [HasPermission(Permission.UpdateUser)]
        [Logging(LoggingType.Full)]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserRequest request)
        {
            request.UserAccountID = userId;
            Result result = await adminService.UpdateUser(request);

            return result.IsSuccess ? Ok() : result.ToProblemDetails();
        }
    }
}
