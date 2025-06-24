using Application.Common.Requests.Company;
using Application.Common.Results;
using Application.Common.Security;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Domain.DTO.Company;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class CompanyFAQService :ICompanyFAQService
    {
        private readonly ICompanyFAQRepository companyFAQRepository;
        private readonly ICompanyFAQCategoryRepository companyFAQCategoryRepository;
        private readonly CompanyAccessGuard companyAccessGuard;
        private readonly int FAQLimitPerCategory;

        public CompanyFAQService(
            ICompanyFAQRepository companyFAQRepository,
            ICompanyFAQCategoryRepository companyFAQCategoryRepository,
            CompanyAccessGuard companyAccessGuard,
            IConfiguration configuration)
        {
            this.companyFAQRepository = companyFAQRepository;
            this.companyFAQCategoryRepository = companyFAQCategoryRepository;
            this.companyAccessGuard = companyAccessGuard;
            FAQLimitPerCategory = configuration.GetValue<int>("CompanyLimits:FAQLimitPerCategory");
        }

        public async Task<Result> Create(int routeCompanyId, CompanyFAQCreateRequest request)
        {
            var accessError = await companyAccessGuard.EnsureAccessToCompany(routeCompanyId);
            if (accessError != Error.None)
                return Result.Failure(accessError);

            // Check limit
            var faqCountPerCategory = await companyFAQRepository.Count(request.CategoryID);
            if (faqCountPerCategory >= FAQLimitPerCategory)
            {
                return Result.Failure(CompanyResults.FAQLimitReached);
            }
            // Check if category exists and belongs to correct company
            var category = await companyFAQCategoryRepository.Get(request.CategoryID);
            if (category == null || category.CompanyID != routeCompanyId)
                return Result.Failure(GenericResults.NotFound);

            var companyFAQ = request.MapToEntity();
            await companyFAQRepository.Add(companyFAQ);

            return Result.Success(CompanyResults.FAQCreated);
        }
        public async Task<Result> Delete(int routeCompanyId, int routeCategoryId, int id)
        {
            var accessError = await companyAccessGuard.EnsureAccessToCompany(routeCompanyId);
            if (accessError != Error.None)
                return Result.Failure(accessError);

            var companyFAQ = await companyFAQRepository.GetFull(id);
            if (companyFAQ == null)
                return Result.Failure(GenericResults.NotFound);

            // Ensure FAQ belongs to correct category and company
            if (companyFAQ.CategoryID != routeCategoryId || companyFAQ.Category.CompanyID != routeCompanyId)
                return Result.Failure(GenericResults.IDMismatch);



            await companyFAQRepository.Delete(companyFAQ);

            return Result.Success(CompanyResults.FAQDeleted);
        }
        public async Task<Result<IEnumerable<CompanyFAQDTO>>> GetAll(int companyId, int categoryID)
        {
            var companyFAQs = await companyFAQRepository.GetAll(companyId, categoryID);
            var companyFAQsDTOs = companyFAQs.Select(faq => faq.MapToDTO());

            return Result.Success(companyFAQsDTOs);
        }

        public async Task<Result> Update(int routeCompanyId, int routeCategoryId, CompanyFAQUpdateRequest request)
        {
            var accessError = await companyAccessGuard.EnsureAccessToCompany(routeCompanyId);
            if (accessError != Error.None)
                return Result.Failure(accessError);

            var companyFAQ = await companyFAQRepository.GetFull(request.ID);
            if (companyFAQ == null)
            {
                return Result.Failure(GenericResults.NotFound);
            }
            // Validate that category and company match
            if (companyFAQ.CategoryID != routeCategoryId || companyFAQ.Category.CompanyID != routeCompanyId)
                return Result.Failure(GenericResults.IDMismatch);

            request.MapToEntity(companyFAQ);
            await companyFAQRepository.Update(companyFAQ);

            return Result.Success(CompanyResults.FAQUpdated);
        }
    }
}
