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
    [Route("api/company")]
    [ApiController]
    [Tags("Company Management")]
    public class CompanyController :ControllerBase
    {
        private readonly ICompanyService companyService;

        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;

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
        [HttpPost("invite")]
        [HasPermission(Permission.CompanyUpdate)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<Result<string>>), StatusCodes.Status200OK)]
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
        [HttpGet("invite-accept")]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<RegisterResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InviteAccept([FromQuery] string token)
        {
            var result = await companyService.InviteAccept(token);

            return result.ToResponse();
        }
        /// <summary>
        /// Creates a list of services offered by the company.
        /// </summary>
        /// <remarks>
        /// Can only be done once if services do not already exist for the company.
        /// </remarks>
        /// <param name="request">List of service details to create.</param>
        /// <returns>Success if services are saved.</returns>
        [HttpPost("service")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.ServiceCreate)]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CompanyServicesSCreate([FromBody] ServicesCreateRequest request)
        {
            Result result = await companyService.ServicesCreate(request);

            return result.ToResponse();
        }
        /// <summary>
        /// Updates existing services for the company.
        /// </summary>
        /// <remarks>
        /// The provided service list must match existing service IDs.
        /// </remarks>
        /// <param name="request">List of service updates.</param>
        /// <returns>Success if updates are applied.</returns>
        [HttpPut("service")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.ServiceUpdate)]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CompanyServicesUpdate([FromBody] ServicesUpdateRequest request)
        {
            Result result = await companyService.ServicesUpdate(request);

            return result.ToResponse();
        }
        /// <summary>
        /// Deletes a specific service from the company's service list.
        /// </summary>
        /// <param name="ID">The unique ID of the service to delete.</param>
        /// <returns>Success if deletion is successful.</returns>
        [HttpDelete("service")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.ServiceDelete)]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CompanyServiceDelete([FromQuery] int ID)
        {
            Result result = await companyService.ServicesDelete(ID);

            return result.ToResponse();
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
        [ProducesResponseType(typeof(SuccessResponse<Result<PagedList<CompanyDTO>>>), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(SuccessResponse<Result<CompanyDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAction(int id)
        {
            var result = await companyService.Get(id, forPublic: true);

            return result.ToResponse();
        }
    }
}
