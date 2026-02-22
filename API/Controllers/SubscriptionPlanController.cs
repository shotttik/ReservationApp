using API.Attributes;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.DTO.Company;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/subscription-plans")]
    [ApiController]
    public class SubscriptionPlanController :ControllerBase
    {
        private readonly ISubscriptionPlanService _subscriptionPlanService;

        public SubscriptionPlanController(ISubscriptionPlanService subscriptionPlanService)
        {
            _subscriptionPlanService = subscriptionPlanService;
        }

        /// <summary>
        /// Retrieves a paginated list of subscription plans.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>Accessible by everyone</strong><br/><br/>
        /// </remarks>
        /// <returns>Paged list of subscription plans.</returns>
        [HttpGet()]
        [Logging(LoggingType.General)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<PagedList<CompanyDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RetrievePagedUsers()
        {
            var result = await _subscriptionPlanService.GetAll();

            return result.ToResponse();
        }
    }
}
