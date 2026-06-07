using API.Attributes;
using Application.Authentication;
using Application.Common.Requests.Promo;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.DTO;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/companies/{companyId:int}/promocodes")]
    [ApiController]
    [Tags("Promo Codes")]
    public class PromoController :ControllerBase
    {
        private readonly IPromoService _promoService;

        public PromoController(IPromoService promoService)
        {
            _promoService = promoService;
        }

        /// <summary>
        /// Validates a promo code and calculates discount for a given booking amount.
        /// </summary>
        /// <remarks>
        /// This endpoint does NOT apply the promo to any booking. It only validates and calculates discount.
        /// </remarks>
        /// <param name="companyId">Company identifier that owns the promo code.</param>
        /// <param name="code">Promo code entered by the user.</param>
        /// <param name="serviceId">service id that should be used for booking.</param>
        /// <returns>Returns validation result including discount amount if applicable.</returns>
        [HttpGet("validate")]
        [EnableRateLimiting("fixed")]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<PromoCodeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ValidateApplyPromo([FromRoute] int companyId, [FromQuery] string code, [FromQuery] int serviceId)
        {
            var result = await _promoService.ValidateApplyPromo(code, companyId, serviceId);
            return result.ToResponse();
        }

        /// <summary>
        /// Creates a new promo code for a company.
        /// </summary>
        /// <remarks>
        /// Required Role: <strong>SuperAdmin, CompanyAdmin</strong>
        /// </remarks>
        /// <param name="companyId">Company under which the promo code will be created.</param>
        /// <param name="request">Promo code creation data including discount rules and validity period.</param>
        /// <returns>Returns created promo code details.</returns>
        [HttpPost()]
        [EnableRateLimiting("fixed")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.PromoCodeCreate)]
        [ProducesResponseType(typeof(SuccessResponse<PromoCodeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromRoute] int companyId, [FromBody] PromoCodeCreateRequest request)
        {
            var result = await _promoService.Create(companyId, request);
            return result.ToResponse();
        }

        /// <summary>
        /// Updates an existing promo code.
        /// </summary>
        /// <remarks>
        /// Required Role: <strong>SuperAdmin, CompanyAdmin</strong>
        /// </remarks>
        /// <param name="companyId">Company that owns the promo code.</param>
        /// <param name="id">Promo code identifier.</param>
        /// <param name="request">Updated promo code data.</param>
        /// <returns>Returns updated promo code information.</returns>
        [HttpPut("{id:int}")]
        [EnableRateLimiting("fixed")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.PromoCodeUpdate)]
        [ProducesResponseType(typeof(SuccessResponse<PromoCodeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] int companyId, [FromRoute] int id, [FromBody] PromoCodeUpdateRequest request)
        {
            var result = await _promoService.Update(id, companyId, request);
            return result.ToResponse();
        }

        /// <summary>
        /// Deletes a promo code permanently (super admin only).
        /// </summary>
        /// <remarks>
        /// This operation performs a hard delete. The promo code will no longer be available for validation or future use.
        /// Required Role: <strong>SuperAdmin</strong>
        /// </remarks>
        /// <param name="companyId">Company that owns the promo code.</param>
        /// <param name="id">Promo code identifier.</param>
        /// <returns>Returns success or failure of the deletion operation.</returns>
        [HttpDelete("{id:int}")]
        [EnableRateLimiting("fixed")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.PromoCodeDelete)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int companyId, [FromRoute] int id)
        {
            var result = await _promoService.Delete(id, companyId);
            return result.ToResponse();
        }

        /// <summary>
        /// Retrieves a paginated list of company promos for the authenticated CompanyAdmin or SuperAdmin.
        /// </summary>
        /// <remarks>
        /// This endpoint returns paginated company promos for the authenticated user's company.  
        /// Required role: <strong>CompanyAdmin, SuperAdmin</strong>
        /// </remarks>
        /// <param name="companyId">The ID of the company in the route.</param>
        /// <param name="parameters">Pagination and filtering parameters.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>Paginated list of company employees or an error response.</returns>
        [HttpGet()]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.PromoCodeRead)]
        [ProducesResponseType(typeof(SuccessResponse<PagedList<PromoCodeDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RetrievePagedEmployees(
            [FromRoute] int companyId,
            [FromQuery] PagedParameters parameters,
            CancellationToken cancellationToken)
        {
            var result = await _promoService.RetrievePaged(companyId, parameters, cancellationToken);
            return result.ToResponse();
        }
    }
}
