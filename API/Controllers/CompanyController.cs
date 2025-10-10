using API.Attributes;
using Application.Authentication;
using Application.Common.Requests.Company;
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
    [Route("api/v{version:apiVersion}/companies")]
    [ApiController]
    [Tags("Companies")]
    public class CompanyController :ControllerBase
    {
        private readonly ICompanyService companyService;

        public CompanyController(
            ICompanyService companyService)
        {
            this.companyService = companyService;
        }
        /// <summary>
        /// Retrieves a paginated list of companies.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>Accessible by everyone</strong><br/><br/>
        /// <b>Paging and filtering parameters:</b><br/>
        /// <b>Sortable / Filterable Fields:</b>
        /// <ul>
        /// <li><c>ID</c></li>
        /// <li><c>Name</c></li>
        /// <li><c>Description</c></li>
        /// <li><c>IN</c></li>
        /// <li><c>Email</c></li>
        /// <li><c>Phone</c></li>
        /// <li><c>Type</c></li>
        /// <li><c>CreatedAt</c></li>
        /// </ul>
        /// </remarks>
        /// <param name="parameters">Pagination parameters including page number, size, and search filters.</param>
        /// <param name="cancellationToken">Request cancellation token.</param>
        /// <returns>Paged list of company records.</returns>
        [HttpGet("paged")]
        [Logging(LoggingType.General)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<PagedList<CompanyDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RetrievePaged([FromQuery] PagedParameters parameters, CancellationToken cancellationToken)
        {
            var result = await companyService.RetrievePaged(parameters, cancellationToken, forPublic: true);

            return result.ToResponse();
        }
        /// <summary>
        /// Retrieves detailed information about a specific company.
        /// </summary>
        /// <param name="id">The ID of the company to retrieve.</param>
        /// <returns>Returns the company details if found.</returns>
        /// <remarks>Required role: <strong>Accessible by everyone</strong></remarks>
        [HttpGet("{id:int}")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<CompanyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await companyService.Get(id, forPublic: true);

            return result.ToResponse();
        }
        /// <summary>
        /// Sends a company employeeship invitation to a user.
        /// </summary>
        /// <remarks>
        /// Only company admins can invite users to join their company. 
        /// The user must currently be a public user.
        /// Required role: <strong>CompanyAdmin</strong>
        /// </remarks>
        /// <param name="request">Contains the user account ID to invite.</param>
        /// <returns>A secure token (for dev/testing) or email notification result.</returns>
        [MapToApiVersion("1.0")]
        [HttpPost("invitations")]
        [HasPermission(Permission.InvitationSend)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InviteEmployee([FromBody] InviteEmployeeRequest request)
        {
            var result = await companyService.InviteEmployee(request.UserAccountID);

            return result.ToResponse();
        }
        /// <summary>
        /// Accepts a company invitation using a secure token.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>Accessible by everyone</strong>
        /// </remarks>
        /// <param name="token">The invitation token received by email.</param>
        /// <returns>Success result if invitation is valid and accepted.</returns>
        [HttpGet("invitations/accept")]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<RegisterResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InviteAccept([FromQuery] string token)
        {
            var result = await companyService.InviteAccept(token);

            return result.ToResponse();
        }
        /// <summary>
        /// Uploads one or more media files for the specified company.
        /// </summary>
        /// <remarks>
        /// This endpoint allows uploading company multiple media files.
        ///
        /// <para><b>Required Roles:</b> SuperAdmin, CompanyAdmin</para>
        /// <para><b>Max File Size:</b> 1 MB (1,048,576 bytes)</para>
        /// <para><b>Allowed File Types:</b> image/jpeg, image/png</para>
        /// </remarks>
        /// <param name="request">The request containing the media files to upload.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A result returns address of the image or failure of the upload operation.</returns>
        [HttpPost("medias")]
        [HasPermission(Permission.CompanyUpdate)]
        [Logging(LoggingType.General)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<List<int>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadMedia([FromBody] UploadCompanyMediasRequest request, CancellationToken cancellationToken)
        {
            var result = await companyService.UploadMedia(request, cancellationToken);

            return result.ToResponse();
        }

        /// <summary>
        /// Updates the description of the authenticated user's company.
        /// </summary>
        /// <remarks>
        /// This endpoint allows a CompanyAdmin to update the <c>Description</c> field of their own company.  
        /// The company is determined from the route param context.
        /// Required role: <strong>SuperAdmin,CompanyAdmin</strong>
        /// </remarks>
        /// <param name="companyId">The ID of the target company for which the employee is being created.</param>
        /// <param name="request">The request containing the new description.</param>
        /// <returns>No content on success; appropriate error response on failure.</returns>
        [HttpPatch("{companyId:int}")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.CompanyUpdatePartial)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PartialUpdate([FromRoute] int companyId, [FromBody] CompanyPartialUpdateRequest request)
        {
            var result = await companyService.Update(companyId, request);
            return result.ToResponse();
        }
        /// <summary>
        /// Creates a new company employee user account for the specified company.
        /// </summary>
        /// <remarks>
        /// This endpoint allows a CompanyAdmin or SuperAdmin to create a new user account (CompanyEmployee role)
        /// for a specific company identified by <paramref name="companyId"/>.  
        /// 
        /// The request must contain all required information including personal details and login credentials.  
        /// Email addresses must be unique in the system.  
        /// A verification token is automatically generated and assigned to the new user.
        /// Required role: <strong>CompanyAdmin, SuperAdmin</strong>
        /// </remarks>
        /// <param name="companyId">The ID of the target company for which the employee is being created.</param>
        /// <param name="request">The request containing new employee details and login credentials.</param>
        /// <returns>
        /// Success response if the user is created; appropriate error response if email already exists
        /// or access is denied.
        /// </returns>
        [HttpPost("{companyId:int}/employees")]
        [HasPermission(Permission.CompanyEmployeeCreate)]
        [Logging(LoggingType.General)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateEmployee(
            [FromRoute] int companyId,
            [FromBody] EmployeeCreateRequest request)
        {
            var result = await companyService.CreateEmployee(companyId, request);
            return result.ToResponse();
        }

        /// <summary>
        /// Updates the profile details of a company employee.
        /// </summary>
        /// <remarks>
        /// This endpoint allows a CompanyAdmin to update an existing employee’s first name, last name, gender, and date of birth.
        /// Only authenticated users with appropriate access to the specified company can perform this action.
        /// The employee is identified by their UserLoginData ID.
        /// Required role: <strong>CompanyAdmin, SuperAdmin</strong>
        /// </remarks>
        /// <param name="companyId">The ID of the company in the route.</param>
        /// <param name="request">The update request containing new profile data.</param>
        /// <returns>No content on success; appropriate error response on failure.</returns>
        [HttpPatch("{companyId:int}/employees")]
        [HasPermission(Permission.CompanyEmployeeUpdate)]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEmployee(
            [FromRoute] int companyId,
            [FromBody] EmployeeUpdateRequest request)
        {
            var result = await companyService.UpdateEmployee(companyId, request);
            return result.ToResponse();
        }

        /// <summary>
        /// Deletes a company employee from the specified company.
        /// </summary>
        /// <remarks>
        /// This endpoint allows a CompanyAdmin to soft delete or permanently delete a company employee (with role CompanyEmployee).  
        /// Only authenticated users with access to the specified company can perform this action.  
        /// The deletion can be a soft delete (default) or a force delete (permanent).
        /// Required role: <strong>CompanyAdmin, SuperAdmin</strong>
        /// </remarks>
        /// <param name="companyId">The ID of the company in the route.</param>
        /// <param name="employeeID">The ID of the employee to delete (UserLoginData ID).</param>
        /// <param name="force">Whether to permanently delete the employee (true) or perform a soft delete (false).</param>
        /// <returns>No content on success; appropriate error response on failure.</returns>
        [HttpDelete("{companyId:int}/employees/{employeeID:int}")]
        [HasPermission(Permission.CompanyEmployeeDelete)]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEmployee(
            [FromRoute] int companyId,
            [FromRoute] int employeeID,
            [FromQuery] bool force = false)
        {
            var result = await companyService.DeleteEmployee(companyId, employeeID, force);
            return result.ToResponse();
        }

        /// <summary>
        /// Retrieves a paginated list of company employees for the authenticated CompanyAdmin.
        /// </summary>
        /// <remarks>
        /// This endpoint returns paginated company employees for the authenticated user's company.  
        /// Required role: <strong>CompanyAdmin, SuperAdmin</strong>
        /// </remarks>
        /// <param name="companyId">The ID of the company in the route.</param>
        /// <param name="parameters">Pagination and filtering parameters.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>Paginated list of company employees or an error response.</returns>
        [HttpGet("{companyId:int}/employees")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.CompanyEmployeeRead)]
        [ProducesResponseType(typeof(SuccessResponse<PagedList<UserLoginDataDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RetrievePagedEmployees(
            [FromRoute] int companyId,
            [FromQuery] PagedParameters parameters,
            CancellationToken cancellationToken)
        {
            var result = await companyService.RetrievePagedCompanyEmployees(companyId, parameters, cancellationToken);
            return result.ToResponse();
        }
        /// <summary>
        /// Updates media for the company.
        /// </summary>
        /// <remarks>
        /// This endpoint allows you to update media for a company, including:
        /// - Adding new media
        /// - Updating existing media (including marking one image as the main media)
        /// - Removing media not included in the update list
        ///
        /// **Business rules:**
        /// - Exactly one media must be marked as the main image (`IsMain = true`) in the request.
        /// - Media not included in the request will be removed from the company.
        /// 
        /// Required role: <strong>CompanyAdmin, SuperAdmin</strong>
        /// </remarks>
        /// <param name="companyId">The ID of the company to update media for.</param>
        /// <param name="mediaUpdates">A list of media update requests that include file uploads, changes to 'main' status, or removal instructions.</param>
        /// <returns>Result indicating success or failure of the operation.</returns>
        [HttpPut("{companyId}/media")]
        [HasPermission(Permission.CompanyMediaUpdate)]
        [Logging(LoggingType.General)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMedia([FromRoute] int companyId, [FromBody] List<UpdateCompanyMediaRequest> mediaUpdates)
        {
            var result = await companyService.UpdateMedia(companyId, mediaUpdates);
            return result.ToResponse();
        }
    }
}
