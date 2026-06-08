using API.Attributes;
using Application.Authentication;
using Application.Common.Requests.Company;
using Application.Common.Results;
using Application.Interfaces;
using Domain.DTO.Company;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/companies")]
    [ApiController]
    [Tags("Companies FAQs")]
    public class CompanyFAQController :ControllerBase
    {
        private readonly ICompanyFAQService companyFAQService;
        private readonly ICompanyFAQCategoryService companyFAQCategoryService;

        /// <summary>  
        /// Initializes a new instance of the <see cref="CompanyFAQController"/> class.  
        /// </summary>  
        /// <param name="companyFAQService">Service for managing FAQs.</param>  
        /// <param name="companyFAQCategoryService">Service for managing FAQ categories.</param>  
        public CompanyFAQController(
            ICompanyFAQService companyFAQService,
            ICompanyFAQCategoryService companyFAQCategoryService)
        {
            this.companyFAQService = companyFAQService;
            this.companyFAQCategoryService = companyFAQCategoryService;
        }

        /// <summary>  
        /// Creates a new FAQ category for a company.  
        /// </summary>  
        /// <remarks>
        /// Required role: <strong>SuperAdmin,CompanyAdmin</strong><br/><br/>
        /// Required permission: <strong>FaqCategoryCreate</strong>
        /// </remarks>
        /// <param name="companyId">The ID of the company.</param>  
        /// <param name="request">The request containing category details.</param>  
        /// <returns>Success result if the category is created successfully.</returns>  
        [HttpPost("{companyId:int}/faq-categories")]
        [HasPermission(Permission.FaqCategoryCreate)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateFAQCategory(int companyId, [FromBody] CompanyFAQCategoryCreateRequest request)
        {
            var result = await companyFAQCategoryService.Create(companyId, request);
            return result.ToResponse();
        }

        /// <summary>  
        /// Updates an existing FAQ category for a company.  
        /// </summary>  
        /// <remarks>
        /// Required role: <strong>SuperAdmin,CompanyAdmin</strong><br/><br/>
        /// Required permission: <strong>FaqCategoryUpdate</strong>
        /// </remarks>
        /// <param name="companyId">The ID of the company.</param>  
        /// <param name="request">The request containing updated category details.</param>  
        /// <returns>Success result if the category is updated successfully.</returns>  
        [HttpPut("{companyId:int}/faq-categories")]
        [HasPermission(Permission.FaqCategoryUpdate)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateFAQCategory(int companyId, [FromBody] CompanyFAQCategoryUpdateRequest request)
        {
            var result = await companyFAQCategoryService.Update(companyId, request);
            return result.ToResponse();
        }

        /// <summary>  
        /// Deletes an FAQ category for a company.  
        /// </summary>  
        /// <remarks>
        /// Required role: <strong>SuperAdmin,CompanyAdmin</strong><br/><br/>
        /// Required permission: <strong>FaqCategoryDelete</strong>
        /// </remarks>
        /// <param name="companyId">The ID of the company.</param>  
        /// <param name="id">The ID of the FAQ category to delete.</param>  
        /// <returns>Success result if the category is deleted successfully.</returns>  
        [HttpDelete("{companyId:int}/faq-categories/{id:int}")]
        [HasPermission(Permission.FaqCategoryDelete)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteFAQCategory(int companyId, int id)
        {
            var result = await companyFAQCategoryService.Delete(companyId, id);
            return result.ToResponse();
        }

        /// <summary>  
        /// Retrieves all FAQ categories for a company.  
        /// </summary>  
        /// <remarks>
        /// Required role: <strong>Accessible by everyone</strong>
        /// </remarks>
        /// <param name="companyId">The ID of the company.</param>  
        /// <returns>A list of FAQ categories.</returns>  
        [HttpGet("{companyId:int}/faq-categories")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<CompanyFAQCategoryDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllFAQCategories(int companyId)
        {
            var result = await companyFAQCategoryService.GetAll(companyId);
            return result.ToResponse();
        }

        /// <summary>  
        /// Creates a new FAQ within a specific category for a company.  
        /// </summary>  
        /// <remarks>
        /// Required role: <strong>SuperAdmin,CompanyAdmin</strong><br/><br/>
        /// Required permission: <strong>FaqCreate</strong>
        /// </remarks>
        /// <param name="companyId">The ID of the company.</param>  
        /// <param name="request">The request containing FAQ details.</param>  
        /// <returns>Success result if the FAQ is created successfully.</returns>  
        [HttpPost("{companyId:int}/faqs")]
        [HasPermission(Permission.FaqCreate)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateFAQ(int companyId, [FromBody] CompanyFAQCreateRequest request)
        {
            var result = await companyFAQService.Create(companyId, request);
            return result.ToResponse();
        }

        /// <summary>  
        /// Updates an existing FAQ within a specific category for a company.  
        /// </summary>  
        /// <remarks>
        /// Required role: <strong>SuperAdmin,CompanyAdmin</strong><br/><br/>
        /// Required permission: <strong>FaqUpdate</strong>
        /// </remarks>
        /// <param name="companyId">The ID of the company.</param>  
        /// <param name="request">The request containing updated FAQ details.</param>  
        /// <returns>Success result if the FAQ is updated successfully.</returns>  
        [HttpPut("{companyId:int}/faqs")]
        [HasPermission(Permission.FaqUpdate)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateFAQ(int companyId, [FromBody] CompanyFAQUpdateRequest request)
        {
            var result = await companyFAQService.Update(companyId, request);
            return result.ToResponse();
        }

        /// <summary>  
        /// Deletes an FAQ within a specific category for a company.  
        /// </summary>  
        /// <remarks>
        /// Required role: <strong>SuperAdmin,CompanyAdmin</strong>.<br/><br/>
        /// Required permission: <strong>FaqDelete</strong>
        /// </remarks>
        /// <param name="companyId">The ID of the company.</param>  
        /// <param name="id">The ID of the FAQ to delete.</param>  
        /// <returns>Success result if the FAQ is deleted successfully.</returns>  
        [HttpDelete("{companyId:int}/faqs/{id:int}")]
        [HasPermission(Permission.FaqDelete)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteFAQ(int companyId, int id)
        {
            var result = await companyFAQService.Delete(companyId, id);
            return result.ToResponse();
        }

        /// <summary>  
        /// Retrieves all FAQs within a specific category for a company.  
        /// </summary>  
        /// <remarks>
        /// Required role: <strong>Accessible by everyone</strong>
        /// </remarks>
        /// <param name="companyId">The ID of the company.</param>  
        /// <param name="categoryId">The ID of the FAQ category.</param>  
        /// <returns>A list of FAQs.</returns>  
        [HttpGet("{companyId:int}/faqs")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<CompanyFAQDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllFAQs(int companyId, [FromQuery] int? categoryId = null)
        {
            var result = await companyFAQService.GetAll(companyId, categoryId);
            return result.ToResponse();
        }
    }
}
