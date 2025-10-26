using API.Attributes;
using Application.Authentication;
using Application.Common.Requests.Company;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/companies/{companyId:int}/services")]
    [ApiController]
    [Tags("Company Services")]
    public class ServicesController :ControllerBase
    {
        private readonly ICompanyService companyService;

        public ServicesController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        /// <summary>
        /// Creates a list of services offered by the company.
        /// </summary>
        /// <remarks>
        /// Can only be done once if services do not already exist for the company.
        /// Required role: <strong>SuperAdmin, CompanyAdmin</strong>
        /// </remarks>
        /// <param name="companyId">The unique ID of the company.</param>
        /// <param name="request">List of service details to create.</param>
        /// <returns>Success if services are saved.</returns>
        [HttpPost]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.ServiceCreate)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CompanyServicesSCreate(int companyId, [FromBody] ServicesCreateRequest request)
        {
            var result = await companyService.CreateServices(companyId, request);

            return result.ToResponse();
        }

        /// <summary>
        /// Updates existing services for the company.
        /// </summary>
        /// <remarks>
        /// The provided service list must match existing service IDs.
        /// Required role: <strong>SuperAdmin, CompanyAdmin</strong>
        /// /// </remarks>
        /// <param name="companyId">The unique ID of the company.</param>
        /// <param name="request">List of service updates.</param>
        /// <returns>Success if updates are applied.</returns>
        [HttpPut]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.ServiceUpdate)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CompanyServicesUpdate(int companyId, [FromBody] ServicesUpdateRequest request)
        {
            var result = await companyService.UpdateServices(companyId, request);

            return result.ToResponse();
        }

        /// <summary>  
        /// Deletes a specific service from the company's service list.  
        /// </summary>  
        /// <remarks>  
        /// Required role: <strong>SuperAdmin,CompanyAdmin</strong>.
        /// </remarks>  
        /// <param name="companyId">The unique ID of the company.</param>  
        /// <param name="ID">The unique ID of the service to delete.</param>  
        /// <returns>Success if deletion is successful.</returns>  
        [HttpDelete("{ID:int}")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.ServiceDelete)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CompanyServiceDelete(int companyId, int ID)
        {
            var result = await companyService.DeleteServices(companyId, ID);

            return result.ToResponse();
        }
        /// <summary>
        /// Retrieves all publicly available services for the specified company.
        /// </summary>
        /// <remarks>
        /// Returns a list of <strong>active</strong> company services available to the public.
        /// <br/><br/>
        /// <b>Access:</b> Open to all users (no authentication required)
        /// </remarks>
        /// <param name="companyId">Unique identifier of the company.</param>
        /// <response code="200">List of active public services or an empty list if none exist.</response>
        [HttpGet("public")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CompanyServicesGet(int companyId)
        {
            var result = await companyService.RetrieveServices(companyId, true);
            return result.ToResponse();
        }
        /// <summary>
        /// Retrieves all services (active and inactive) for the specified company.
        /// </summary>
        /// <remarks>
        /// Returns the complete list of services, regardless of their active status.
        /// <br/><br/>
        /// <b>Required Roles:</b> SuperAdmin, CompanyAdmin
        /// </remarks>
        /// <param name="companyId">Unique identifier of the company.</param>
        /// <response code="200">List of all company services or an empty list if none exist.</response>
        [HttpGet]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.ServiceRead)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CompanyActiveServicesGet(int companyId)
        {
            var result = await companyService.RetrieveServices(companyId, false);
            return result.ToResponse();
        }
    }
}
