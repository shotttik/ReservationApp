using API.Attributes;
using Application.Authentication;
using Application.Common.Requests.Admin;
using Application.Common.Responses;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.DTO.User;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

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
            var result = await adminService.UserCreate(request);

            return result.ToResponse();
        }

        [HttpPatch("user/{id}")]
        [HasPermission(Permission.UserUpdate)]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UserUpdate(int id, [FromBody] UserUpdateRequest request)
        {
            request.ID = id;
            var result = await adminService.UserUpdate(request);

            return result.ToResponse();
        }

        [HttpPost("company")]
        [HasPermission(Permission.CompanyCreate)]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CompanyCreate([FromBody] CompanyCreateRequest request)
        {
            var result = await adminService.CompanyCreate(request);

            return result.ToResponse();
        }
        [HttpGet("users/paged")]
        [HasPermission(Permission.UserRead)]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<Result<PagedList<AuthUser>>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RetrievePagedUsers([FromQuery] PagedParameters request, CancellationToken cancellationToken)
        {
            var result = await adminService.RetrievePagedUsers(request, cancellationToken);

            return result.ToResponse();
        }
        [HttpPost("assign-user-to-company")]
        [HasPermission(Permission.UserUpdate)]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AssignUserToCompany([FromBody] AssignUserToCompanyRequest request)
        {
            var result = await adminService.AssignUserToCompany(request);

            return result.ToResponse();
        }
    }
}
