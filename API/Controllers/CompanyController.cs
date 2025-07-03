using API.Attributes;
using Application.Authentication;
using Application.Common.Requests.Company;
using Application.Common.Responses;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.DTO.Company;
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
        /// <param name="parameters">Pagination parameters including page number, size, and search filters.</param>
        /// <param name="cancellationToken">Request cancellation token.</param>
        /// <returns>Paged list of company records.</returns>
        [HttpGet("paged")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<PagedList<CompanyDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RetrievePaged([FromQuery] PagedParameters parameters, CancellationToken cancellationToken)
        {
            var result = await companyService.RetrievePaged(parameters, cancellationToken, forPublic: false);

            return result.ToResponse();
        }
        /// <summary>
        /// Retrieves detailed information about a specific company.
        /// </summary>
        /// <param name="id">The ID of the company to retrieve.</param>
        /// <returns>Returns the company details if found.</returns>
        [HttpGet("{id:int}")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<CompanyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCompany(int id)
        {
            var result = await companyService.Get(id, forPublic: true);

            return result.ToResponse();
        }
        /// <summary>
        /// Sends a company membership invitation to a user.
        /// </summary>
        /// <remarks>
        /// Only company admins can invite users to join their company. 
        /// The user must currently be a public user.
        /// </remarks>
        /// <param name="request">Contains the user account ID to invite.</param>
        /// <returns>A secure token (for dev/testing) or email notification result.</returns>
        [MapToApiVersion("1.0")]
        [HttpPost("invitations")]
        [HasPermission(Permission.CompanyUpdate)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InviteMember([FromBody] InviteMemberRequest request)
        {
            var result = await companyService.InviteMember(request.UserAccountID);

            return result.ToResponse();
        }
        /// <summary>
        /// Accepts a company invitation using a secure token.
        /// </summary>
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
        /// Uploads images for the company.  
        /// </summary>  
        /// <remarks>  
        /// This endpoint allows uploading multiple images for a company.  
        /// Only users(Company Admin) with the appropriate permissions can perform this action.  
        /// </remarks>  
        /// <param name="request">The request containing the images to upload.</param>
        /// <param name="cancellationToken">Cancellation token</param>  
        /// <returns>Result indicating success or failure of the upload operation.</returns>
        [HttpPost("images/upload")]
        [HasPermission(Permission.CompanyUpdate)]
        [Logging(LoggingType.General)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadImages([FromForm] UploadCompanyImagesRequest request, CancellationToken cancellationToken)
        {
            var result = await companyService.UploadImages(request, cancellationToken);

            return result.ToResponse();
        }

        /// <summary>
        /// Updates the description of the authenticated user's company.
        /// </summary>
        /// <remarks>
        /// This endpoint allows a CompanyAdmin to update the <c>Description</c> field of their own company.  
        /// Only authenticated users with the CompanyAdmin role can perform this action.  
        /// The company is determined from the authenticated user's context.
        /// </remarks>
        /// <param name="request">The request containing the new description.</param>
        /// <returns>No content on success; appropriate error response on failure.</returns>
        [HttpPatch]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.CompanyUpdate)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CompanyPartialUpdate([FromBody] CompanyPartialUpdateRequest request)
        {
            var result = await companyService.Update(request);
            return result.ToResponse();
        }
        /// <summary>
        /// Creates a new company member user account for the specified company.
        /// </summary>
        /// <remarks>
        /// This endpoint allows a CompanyAdmin or SuperAdmin to create a new user account (CompanyMember role)
        /// for a specific company identified by <paramref name="routeCompanyId"/>.  
        /// 
        /// The request must contain all required information including personal details and login credentials.  
        /// Email addresses must be unique in the system.  
        /// A verification token is automatically generated and assigned to the new user.
        /// </remarks>
        /// <param name="routeCompanyId">The ID of the target company for which the member is being created.</param>
        /// <param name="request">The request containing new member details and login credentials.</param>
        /// <returns>
        /// Success response if the user is created; appropriate error response if email already exists
        /// or access is denied.
        /// </returns>
        [HttpPost("{routeCompanyId:int}/members")]
        [HasPermission(Permission.CompanyUpdate)]
        [Logging(LoggingType.General)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateCompanyMember(
            [FromRoute] int routeCompanyId,
            [FromBody] CreateCompanyMemberRequest request)
        {
            var result = await companyService.CreateCompanyMember(routeCompanyId, request);
            return result.ToResponse();
        }
    }
}
