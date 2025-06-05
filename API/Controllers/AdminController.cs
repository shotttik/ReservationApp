using API.Attributes;
using Application.Authentication;
using Application.Common.Requests.Admin;
using Application.Common.Responses;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.DTO.Company;
using Domain.DTO.User;
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
        private readonly ICompanyService companyService;
        private readonly IUserService userService;

        public AdminController(
            IAdminService adminService,
            ICompanyService companyService,
            IUserService userService)
        {
            this.adminService = adminService;
            this.companyService = companyService;
            this.userService = userService;
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

        [HttpDelete("user/{id}")]
        [HasPermission(Permission.UserDelete)]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UserDelete(int id, [FromQuery] bool force = false)
        {
            var result = await userService.Delete(id, force);

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
        [HttpGet("company/paged")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.CompanyRead)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<Result<PagedList<CompanyDTO>>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RetrievePaged([FromQuery] PagedParameters parameters, CancellationToken cancellationToken)
        {
            var result = await companyService.RetrievePaged(parameters, cancellationToken, forPublic: true);

            return result.ToResponse();
        }

        [HttpGet("company/{id:int}")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<Result<CompanyDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAction(int id)
        {
            var result = await companyService.Get(id, forPublic: false);

            return result.ToResponse();
        }
    }
}
