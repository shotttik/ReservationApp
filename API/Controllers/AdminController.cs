using API.Attributes;
using Application.Authentication;
using Application.Common.Requests.Admin;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.DTO.Review;
using Domain.DTO.User;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/admin")]
    [ApiController]
    public class AdminController :ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IUserService _userService;
        private readonly IReviewService _reviewService;
        private readonly IBranchService _branchService;

        public AdminController(
            IAdminService adminService,
            IUserService userService,
            IReviewService reviewService,
            IBranchService branchService)
        {
            _adminService = adminService;
            _userService = userService;
            _reviewService = reviewService;
            _branchService = branchService;
        }
        /// <summary>
        /// Creates a new user under administrator control.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>UserCreate</strong>
        /// </remarks>
        /// <param name="request">User creation data including role and optional company assignment.</param>
        /// <returns>Returns success or failure of the operation.</returns>
        [MapToApiVersion("1.0")]
        [Tags("Administration-User")]
        [HttpPost("users")]
        [HasPermission(Permission.UserCreate)]
        [Logging(LoggingType.ExceptBody)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UserCreate([FromBody] UserCreateRequest request)
        {
            var result = await _adminService.UserCreate(request);

            return result.ToResponse();
        }

        /// <summary>
        /// Updates an existing user's account information.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>UserUpdate</strong>
        /// </remarks>
        /// <param name="id">ID of the user to update.</param>
        /// <param name="request">Partial update payload for user account.</param>
        /// <returns>Returns success or failure of the update.</returns>
        [HttpPatch("users/{id:int}")]
        [Tags("Administration-User")]
        [HasPermission(Permission.UserUpdate)]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UserUpdate(int id, [FromBody] UserUpdateRequest request)
        {
            var result = await _adminService.UserUpdate(id, request);

            return result.ToResponse();
        }
        /// <summary>
        /// Resets password for an existing user.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>UserUpdate</strong>
        /// </remarks>
        /// <param name="id">ID of the user.</param>
        /// <param name="request">request of user password.</param>
        /// <returns>Returns success or failure of the update.</returns>
        [HttpPost("users/{id:int}/reset-password")]
        [Tags("Administration-User")]
        [HasPermission(Permission.UserUpdate)]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AdminResetPassword(int id, [FromBody] AdminResetPasswordRequest request)
        {
            var result = await _adminService.ResetUserPassword(id, request);

            return result.ToResponse();
        }
        /// <summary>
        /// Deletes a user account. Supports soft and hard delete.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>UserDelete</strong>
        /// </remarks>
        /// <param name="id">ID of the user to delete.</param>
        /// <param name="force">Set to true for hard delete; false for soft delete (default).</param>
        /// <returns>Returns result of deletion.</returns>
        [HttpDelete("users/{id}")]
        [Tags("Administration-User")]
        [HasPermission(Permission.UserDelete)]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UserDelete(int id, [FromQuery] bool force = false)
        {
            var result = await _userService.Delete(id, force);

            return result.ToResponse();
        }
        /// <summary>
        /// Creates a new company.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>CompanyCreate</strong>
        /// </remarks>
        /// <param name="request">Company creation payload including name, email, and identifier.</param>
        /// <returns>Returns success or failure of the creation.</returns>
        [HttpPost("companies")]
        [Tags("Administration-Company")]
        [HasPermission(Permission.CompanyCreate)]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CompanyCreate([FromBody] CompanyCreateRequest request)
        {
            var result = await _adminService.CompanyCreate(request);

            return result.ToResponse();
        }
        /// <summary>
        /// Retrieves a paginated list of users with filtering options.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>UserRead</strong><br/><br/>
        /// <b>Paging and filtering parameters:</b><br/>
        /// <b>Sortable / Filterable Fields:</b>
        /// <ul>
        /// <li><c>ID</c></li>
        /// <li><c>CompanyID</c></li>
        /// <li><c>FirstName</c></li>
        /// <li><c>LastName</c></li>
        /// <li><c>Email</c></li>
        /// <li><c>VerificationStatus</c></li>
        /// <li><c>Role.Name</c></li>
        /// <li><c>ActiveStatus</c></li>
        /// <li><c>CreatedAt</c></li>
        /// <li><c>UpdatedAt</c></li>
        /// </ul>
        ///  Filtering Example: name~=Company 4||email~=Company40,id==3107
        /// </remarks>
        /// <param name="request">Paging and filtering parameters.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Paginated list of users.</returns>
        [HttpGet("users")]
        [Tags("Administration-User")]
        [HasPermission(Permission.UserRead)]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<PagedList<AuthUser>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RetrievePagedUsers([FromQuery] PagedParameters request, CancellationToken cancellationToken)
        {
            var result = await _adminService.RetrievePagedUsers(request, cancellationToken);

            return result.ToResponse();
        }
        /// <summary>
        /// Assigns a user to a company with a specified role.
        /// Only CompanyEmployee can have branch.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>UserUpdate</strong>
        /// </remarks>
        /// <param name="request">Assignment data including user ID, company ID, and role.</param>
        /// <returns>Returns assignment result.</returns>
        [HttpPost("assign-user-to-company")]
        [Tags("Administration-Company")]
        [HasPermission(Permission.UserUpdate)]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AssignUserToCompany([FromBody] AssignUserToCompanyRequest request)
        {
            var result = await _adminService.AssignUserToCompany(request);

            return result.ToResponse();
        }
        /// <summary>
        /// Retrieves detailed information about a specific user.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>UserRead</strong>
        /// </remarks>
        /// <param name="id">User ID.</param>
        /// <returns>User details or error response.</returns>
        [HttpGet("users/{id:int}")]
        [Tags("Administration-User")]
        [HasPermission(Permission.UserRead)]
        [EnableRateLimiting("fixed")]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<UserLoginDataDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _adminService.GetUser(id);

            return result.ToResponse();
        }

        /// <summary>
        /// Deletes all active login sessions for a specific user.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>UserDelete</strong>
        /// </remarks>
        /// <param name="id">ID of the user whose active sessions will be deleted.</param>
        /// <returns>Returns the result of the session termination process.</returns>
        [HttpDelete("users/sessions/{id:int}")]
        [Tags("Administration-User")]
        [HasPermission(Permission.UserDelete)]
        [EnableRateLimiting("fixed")]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSpecificUserAllActiveSessions(int id)
        {
            var result = await _userService.DeleteAllActiveSessions(id);

            return result.ToResponse();
        }

        /// <summary>
        /// Updates an existing company's information by ID. Only accessible by SuperAdmin.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>CompanyUpdateFull</strong>
        /// </remarks>
        /// <param name="id">The ID of the company to update.</param>
        /// <param name="request">The updated company information.</param>
        /// <returns>No content if successful, or validation/problem details on failure.</returns>
        [HttpPut("companies/{id:int}")]
        [Tags("Administration-Company")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.CompanyUpdateFull)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] CompanyUpdateRequest request)
        {
            var result = await _adminService.CompanyUpdate(id, request);

            return result.ToResponse();
        }
        /// <summary>
        /// Delete/Disable branch for the company.
        /// </summary>
        /// <remarks>
        /// This endpoint allows you to delete/disable branch for a company
        ///
        /// **Business rules:**
        /// - SuperAdmin able to delete or disable branch
        /// - after disabling company admin is not able to see that branch.
        /// - we are doing this because of avoid deletion of branch and releated bookings (for statistic), 
        /// because they are connected FK cascade delete.
        /// - after deletion employee leaves branch, bookings also deleting. that why soft dalate is good to use if its not mandatory.
        /// 
        /// Required role: <strong>SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>BranchDelete</strong>
        /// </remarks>
        /// <param name="companyId">Company Id</param>
        /// <param name="branchId">Branch Id.</param>
        /// <param name="force">force query parameter indicates soft(false) or hard(true) dalete.</param>
        /// <returns>Result indicating success or failure of the operation.</returns>
        [HttpDelete("companies/{companyId:int}/branches/{branchId:int}")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.BranchDelete)]
        [Tags("Administration-Company")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBranch([FromRoute] int companyId, [FromRoute] int branchId, [FromQuery] bool force)
        {
            var result = await _branchService.Delete(companyId, branchId, force);
            return result.ToResponse();
        }
    }
}
