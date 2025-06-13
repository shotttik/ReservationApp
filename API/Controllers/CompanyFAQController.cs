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
        /// <param name="companyId">The ID of the company.</param>  
        /// <param name="request">The request containing category details.</param>  
        /// <returns>Success result if the category is created successfully.</returns>  
        [HttpPost("{companyId:int}/faq-categories")]
        [HasPermission(Permission.CompanyUpdate)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateFAQCategory([FromBody] CompanyFAQCategoryCreateRequest request)
        {
            var result = await companyFAQCategoryService.Create(request);
            return result.ToResponse();
        }

        /// <summary>  
        /// Updates an existing FAQ category for a company.  
        /// </summary>  
        /// <param name="companyId">The ID of the company.</param>  
        /// <param name="id">The ID of the FAQ category to update.</param>  
        /// <param name="request">The request containing updated category details.</param>  
        /// <returns>Success result if the category is updated successfully.</returns>  
        [HttpPut("{companyId:int}/faq-categories/{id:int}")]
        [HasPermission(Permission.CompanyUpdate)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateFAQCategory(int id, [FromBody] CompanyFAQCategoryUpdateRequest request)
        {
            if (id != request.ID)
                return Result.Failure(GenericResults.IDMismatch).ToProblemDetails();

            var result = await companyFAQCategoryService.Update(request);
            return result.ToResponse();
        }

        /// <summary>  
        /// Deletes an FAQ category for a company.  
        /// </summary>  
        /// <param name="companyId">The ID of the company.</param>  
        /// <param name="id">The ID of the FAQ category to delete.</param>  
        /// <returns>Success result if the category is deleted successfully.</returns>  
        [HttpDelete("{companyId:int}/faq-categories/{id:int}")]
        [HasPermission(Permission.CompanyUpdate)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteFAQCategory(int id)
        {
            var result = await companyFAQCategoryService.Delete(id);
            return result.ToResponse();
        }

        /// <summary>  
        /// Retrieves all FAQ categories for a company.  
        /// </summary>  
        /// <param name="companyId">The ID of the company.</param>  
        /// <returns>A list of FAQ categories.</returns>  
        [HttpGet("{companyId:int}/faq-categories")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<Result<IEnumerable<CompanyFAQCategoryDTO>>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllFAQCategories(int companyId)
        {
            var result = await companyFAQCategoryService.GetAll(companyId);
            return result.ToResponse();
        }

        /// <summary>  
        /// Creates a new FAQ within a specific category for a company.  
        /// </summary>  
        /// <param name="companyId">The ID of the company.</param>  
        /// <param name="categoryId">The ID of the FAQ category.</param>  
        /// <param name="request">The request containing FAQ details.</param>  
        /// <returns>Success result if the FAQ is created successfully.</returns>  
        [HttpPost("{companyId:int}/faq-categories/{categoryId:int}/faqs")]
        [HasPermission(Permission.CompanyUpdate)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateFAQ(int categoryId, [FromBody] CompanyFAQCreateRequest request)
        {
            if (categoryId != request.CategoryID)
                return Result.Failure(GenericResults.IDMismatch).ToProblemDetails();

            var result = await companyFAQService.Create(request);
            return result.ToResponse();
        }

        /// <summary>  
        /// Updates an existing FAQ within a specific category for a company.  
        /// </summary>  
        /// <param name="companyId">The ID of the company.</param>  
        /// <param name="categoryId">The ID of the FAQ category.</param>  
        /// <param name="id">The ID of the FAQ to update.</param>  
        /// <param name="request">The request containing updated FAQ details.</param>  
        /// <returns>Success result if the FAQ is updated successfully.</returns>  
        [HttpPut("{companyId:int}/faq-categories/{categoryId:int}/faqs/{id:int}")]
        [HasPermission(Permission.CompanyUpdate)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateFAQ(int id, int categoryId, [FromBody] CompanyFAQUpdateRequest request)
        {
            if (id != request.ID || categoryId != request.CategoryID)
                return Result.Failure(GenericResults.IDMismatch).ToProblemDetails();

            var result = await companyFAQService.Update(request);
            return result.ToResponse();
        }

        /// <summary>  
        /// Deletes an FAQ within a specific category for a company.  
        /// </summary>  
        /// <param name="companyId">The ID of the company.</param>  
        /// <param name="categoryId">The ID of the FAQ category.</param>  
        /// <param name="id">The ID of the FAQ to delete.</param>  
        /// <returns>Success result if the FAQ is deleted successfully.</returns>  
        [HttpDelete("{companyId:int}/faq-categories/{categoryId:int}faqs/{id:int}")]
        [HasPermission(Permission.CompanyUpdate)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteFAQ(int id)
        {
            var result = await companyFAQService.Delete(id);
            return result.ToResponse();
        }

        /// <summary>  
        /// Retrieves all FAQs within a specific category for a company.  
        /// </summary>  
        /// <param name="companyId">The ID of the company.</param>  
        /// <param name="categoryId">The ID of the FAQ category.</param>  
        /// <returns>A list of FAQs.</returns>  
        [HttpGet("{companyId:int}/faq-categories/{categoryId:int}/faqs")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<Result<IEnumerable<CompanyFAQDTO>>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllFAQs(int categoryId)
        {
            var result = await companyFAQService.GetAll(categoryId);
            return result.ToResponse();
        }

        /// <summary>  
        /// Retrieves a specific FAQ within a category for a company.  
        /// </summary>  
        /// <param name="companyId">The ID of the company.</param>  
        /// <param name="categoryId">The ID of the FAQ category.</param>  
        /// <param name="id">The ID of the FAQ to retrieve.</param>  
        /// <returns>The requested FAQ details.</returns>  
        [HttpGet("{companyId:int}/faq-categories/{categoryId:int}/faqs/{id:int}")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<Result<CompanyFAQDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFAQ(int id)
        {
            var result = await companyFAQService.Get(id);
            return result.ToResponse();
        }
    }
}
