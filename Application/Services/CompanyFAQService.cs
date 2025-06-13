using Application.Common.Requests.Company;
using Application.Common.Results;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Domain.DTO.Company;
using Domain.DTO.User;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    public class CompanyFAQService :ICompanyFAQService
    {
        private readonly ICompanyFAQRepository companyFAQRepository;
        private readonly ICompanyFAQCategoryRepository companyFAQCategoryRepository;
        private readonly IAuthService authService;
        private readonly int FAQLimitPerCategory = 10;

        public CompanyFAQService(
            ICompanyFAQRepository companyFAQRepository,
            ICompanyFAQCategoryRepository companyFAQCategoryRepository,
            IAuthService authService)
        {
            this.companyFAQRepository = companyFAQRepository;
            this.companyFAQCategoryRepository = companyFAQCategoryRepository;
            this.authService = authService;
        }

        public async Task<Result> Create(CompanyFAQCreateRequest request)
        {
            var AuthUser = await authService.GetCurrentUser();
            var faqCountPerCategory = await companyFAQRepository.Count(request.CategoryID);
            if (faqCountPerCategory >= FAQLimitPerCategory)
            {
                return Result.Failure(CompanyResults.FAQLimitReached);
            }
            var companyFAQ = request.MapToEntity();
            var categoryFAQ = await companyFAQCategoryRepository.Get(request.CategoryID);
            if (categoryFAQ == null || categoryFAQ.CompanyID != AuthUser.CompanyID)
            {
                return Result.Failure(GenericResults.NotFound);
            }
            await companyFAQRepository.Add(companyFAQ);

            return Result.Success(CompanyResults.FAQCreated);
        }
        public async Task<Result> Delete(int id)
        {
            var AuthUser = await authService.GetCurrentUser();
            var companyFAQ = await companyFAQRepository.GetFull(id);

            if (companyFAQ == null || companyFAQ.Category.CompanyID != AuthUser.CompanyID!.Value)
            {
                return Result.Failure(GenericResults.NotFound);
            }
            await companyFAQRepository.Delete(companyFAQ);

            return Result.Success(CompanyResults.FAQDeleted);
        }

        public async Task<Result<CompanyFAQDTO>> Get(int companyFAQID)
        {
            var companyFAQ = await companyFAQRepository.Get(companyFAQID);
            if (companyFAQ == null)
            {
                return Result.Failure<CompanyFAQDTO>(GenericResults.NotFound);
            }

            return Result.Success(companyFAQ.MapToDTO());
        }

        public async Task<Result<IEnumerable<CompanyFAQDTO>>> GetAll(int categoryID)
        {
            var companyFAQs = await companyFAQRepository.GetAll(categoryID);
            var companyFAQsDTOs = companyFAQs.Select(faq => faq.MapToDTO());

            return Result.Success(companyFAQsDTOs);
        }

        public async Task<Result> Update(CompanyFAQUpdateRequest request)
        {
            var companyFAQ = await companyFAQRepository.Get(request.ID);
            if (companyFAQ == null)
            {
                return Result.Failure(GenericResults.NotFound);
            }
            request.MapToEntity(companyFAQ);
            await companyFAQRepository.Update(companyFAQ);

            return Result.Success(CompanyResults.FAQUpdated);
        }
    }
}
