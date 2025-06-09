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
    [Route("api/v{version:apiVersion}/services")]
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
        /// </remarks>
        /// <param name="request">List of service details to create.</param>
        /// <returns>Success if services are saved.</returns>
        [HttpPost]
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
        [HttpPut]
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
        [HttpDelete]
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
    }
}
