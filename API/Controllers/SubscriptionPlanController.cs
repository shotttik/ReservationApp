using API.Attributes;
using Application.Authentication;
using Application.Common.Requests.Company;
using Application.Common.Requests.SubscriptionPlan;
using Application.Common.Results;
using Application.Interfaces;
using Application.Services;
using Domain.Abstractions;
using Domain.DTO.Company;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/subscription-plans")]
    [ApiController]
    [Tags("Subscription Plans")]
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


        /// <summary>  
        /// Updates an existing Subscription plan.  
        /// </summary>  
        /// <remarks>
        /// Required role: <strong>SuperAdmin</strong>
        /// </remarks>
        /// <param name="id">The ID of the Subscription plan.</param>  
        /// <param name="request">The request containing updated subscription plan details.</param>  
        /// <returns>Success result if the plan is updated successfully.</returns>  
        [HttpPut("{id:int}")]
        [HasPermission(Permission.SubscriptionPlanUpdate)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateFAQCategory(int id, [FromBody] SubscriptionPlanUpdateRequest request)
        {
            var result = await _subscriptionPlanService.Update(id, request);

            return result.ToResponse();
        }
    }
}
