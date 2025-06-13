using Application.Common.Requests.Company;
using Application.Common.Results;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Domain.DTO.Company;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    public class CompanyFAQCategoryService :ICompanyFAQCategoryService
    {
        private readonly ICompanyFAQCategoryRepository companyFAQCategoryRepository;
        private readonly IAuthService authService;
        private readonly int MaxFAQCategories = 10;
        public CompanyFAQCategoryService(
            ICompanyFAQCategoryRepository companyFAQCategoryRepository,
            IAuthService authService)
        {
            this.companyFAQCategoryRepository = companyFAQCategoryRepository;
            this.authService = authService;
        }

        public async Task<Result> Create(CompanyFAQCategoryCreateRequest request)
        {
            var AuthUser = await authService.GetCurrentUser();

            var companyFAQCategories = await companyFAQCategoryRepository.Count(AuthUser.CompanyID!.Value);
            if (companyFAQCategories >= MaxFAQCategories)
            {
                return Result.Failure(CompanyResults.MaxFAQCategoriesReached);
            }
            var companyFAQCategory = request.MapToEntity();
            companyFAQCategory.CompanyID = AuthUser.CompanyID!.Value;
            await companyFAQCategoryRepository.Add(companyFAQCategory);

            return Result.Success(CompanyResults.FAQCreated);
        }

        public async Task<Result> Delete(int ID)
        {
            var AuthUser = await authService.GetCurrentUser();
            var companyFAQCategory = await companyFAQCategoryRepository.Get(ID);
            if (companyFAQCategory == null || companyFAQCategory.CompanyID != AuthUser.CompanyID)
            {
                return Result.Failure(GenericResults.NotFound);
            }
            await companyFAQCategoryRepository.Delete(companyFAQCategory);

            return Result.Success(CompanyResults.FAQCategoryDeleted);
        }

        public async Task<Result<IEnumerable<CompanyFAQCategoryDTO>>> GetAll(int companyID)
        {
            var companyFAQCategories = await companyFAQCategoryRepository.GetAll(companyID);
            var companyFAQCategoryDTOs = companyFAQCategories.Select(c => c.MapToDTO());

            return Result.Success(companyFAQCategoryDTOs);
        }

        public async Task<Result> Update(CompanyFAQCategoryUpdateRequest request)
        {
            var AuthUser = await authService.GetCurrentUser();
            var companyFAQCategory = await companyFAQCategoryRepository.Get(request.ID);
            if (companyFAQCategory == null || companyFAQCategory.CompanyID != AuthUser.CompanyID)
            {
                return Result.Failure(GenericResults.NotFound);
            }
            request.MapToEntity(companyFAQCategory);

            await companyFAQCategoryRepository.Update(companyFAQCategory);

            return Result.Success(CompanyResults.FAQCategoryUpdated);
        }
    }
}
