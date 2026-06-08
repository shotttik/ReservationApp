using API.Attributes;
using Application.Authentication;
using Application.Common.Requests.SubscriptionPlan;
using Application.Common.Results;
using Application.Interfaces;
using Domain.DTO;
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
        /// Retrieves a list of subscription plans.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>Accessible by everyone</strong><br/><br/>
        /// </remarks>
        /// <returns>List of subscription plans.</returns>
        [HttpGet()]
        [Logging(LoggingType.General)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<List<SubscriptionPlanDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RetrievePagedSubscriptionPlans()
        {
            var result = await _subscriptionPlanService.GetAll();

            return result.ToResponse();
        }

        /// <summary>  
        /// Create an Subscription plan.  
        /// </summary>  
        /// <remarks>
        /// Required role: <strong>SuperAdmin</strong>
        /// Required permission: <strong>SubscriptionPlanCreate</strong>
        /// </remarks>
        /// <param name="request">The request containing subscription plan details.</param>  
        /// <returns>SubscriptionPlanDTO if the plan is created successfully or Error details.</returns>  
        [HttpPost]
        [HasPermission(Permission.SubscriptionPlanCreate)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] SubscriptionPlanCreateRequest request)
        {
            var result = await _subscriptionPlanService.Create(request);

            return result.ToResponse();
        }

        /// <summary>  
        /// Create an Subscription plan.  
        /// </summary>  
        /// <remarks>
        /// Required role: <strong>SuperAdmin</strong>
        /// Required permission: <strong>SubscriptionPlanDelete</strong>
        /// </remarks>
        /// <param name="id">The ID of the Subscription plan.</param>  
        /// <returns>Success result if the plan is delete successfully or error details.</returns>  
        [HttpDelete("{id:int}")]
        [HasPermission(Permission.SubscriptionPlanDelete)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _subscriptionPlanService.Delete(id);

            return result.ToResponse();
        }

        /// <summary>  
        /// Updates an existing Subscription plan.  
        /// </summary>  
        /// <remarks>
        /// Required role: <strong>SuperAdmin</strong>
        /// Required permission: <strong>SubscriptionPlanUpdate</strong>
        /// </remarks>
        /// <param name="id">The ID of the Subscription plan.</param>  
        /// <param name="request">The request containing updated subscription plan details.</param>  
        /// <returns>Success result if the plan is updated successfully or error details.</returns>  
        [HttpPut("{id:int}")]
        [HasPermission(Permission.SubscriptionPlanUpdate)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] SubscriptionPlanUpdateRequest request)
        {
            var result = await _subscriptionPlanService.Update(id, request);

            return result.ToResponse();
        }
    }
}
