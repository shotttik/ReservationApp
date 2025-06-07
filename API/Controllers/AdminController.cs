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
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/super-admin")]
    [ApiController]
    [Tags("Admin Management")]
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
        /// <summary>
        /// Creates a new user under administrator control.
        /// </summary>
        /// <param name="request">User creation data including role and optional company assignment.</param>
        /// <returns>Returns success or failure of the operation.</returns>
        [MapToApiVersion("1.0")]
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

        /// <summary>
        /// Updates an existing user's account information.
        /// </summary>
        /// <param name="id">ID of the user to update.</param>
        /// <param name="request">Partial update payload for user account.</param>
        /// <returns>Returns success or failure of the update.</returns>
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
        /// <summary>
        /// Deletes a user account. Supports soft and hard delete.
        /// </summary>
        /// <param name="id">ID of the user to delete.</param>
        /// <param name="force">Set to true for hard delete; false for soft delete (default).</param>
        /// <returns>Returns result of deletion.</returns>
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
        /// <summary>
        /// Creates a new company.
        /// </summary>
        /// <param name="request">Company creation payload including name, email, and identifier.</param>
        /// <returns>Returns success or failure of the creation.</returns>
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
        /// <summary>
        /// Retrieves a paginated list of users with filtering options.
        /// </summary>
        /// <param name="request">Paging and filtering parameters.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Paginated list of users.</returns>
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
        /// <summary>
        /// Assigns a user to a company with a specified role.
        /// </summary>
        /// <param name="request">Assignment data including user ID, company ID, and role.</param>
        /// <returns>Returns assignment result.</returns>
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
        /// <summary>
        /// Retrieves a paginated list of companies visible to the admin.
        /// </summary>
        /// <param name="parameters">Paging parameters.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Paginated company data.</returns>
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
        /// <summary>
        /// Retrieves detailed information about a company by ID.
        /// </summary>
        /// <param name="id">Company ID.</param>
        /// <returns>Company details or not found error.</returns>
        [HttpGet("company/{id:int}")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<Result<CompanyDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCompany(int id)
        {
            var result = await companyService.Get(id, forPublic: false);

            return result.ToResponse();
        }
        /// <summary>
        /// Retrieves detailed information about a specific user.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <returns>User details or error response.</returns>
        [HttpGet("user/{id:int}")]
        [HasPermission(Permission.UserRead)]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<Result<UserLoginDataDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await adminService.GetUser(id);

            return result.ToResponse();
        }
        /// <summary>
        /// Reactivates a previously soft-deleted user.
        /// </summary>
        /// <param name="id">ID of the user to reactivate.</param>
        /// <returns>Returns result of the reactivation process.</returns>
        [HttpPatch("user/{id}/reactivate")]
        [HasPermission(Permission.UserUpdate)]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<Result<UserLoginDataDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReactivateUser(int id)
        {
            var result = await adminService.ReactivateUser(id);

            return result.ToResponse();
        }
    }
}
