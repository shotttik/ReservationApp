using Application.Common.Requests.Company;
using Application.Common.Results;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Application.Options;
using Domain.DTO.Company;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class CompanyFAQCategoryService :ICompanyFAQCategoryService
    {
        private readonly ICompanyFAQCategoryRepository companyFAQCategoryRepository;
        private readonly IAccessGuard companyAccessGuard;
        private readonly CompanyOptions _companyOptions;
        public CompanyFAQCategoryService(
            ICompanyFAQCategoryRepository companyFAQCategoryRepository,
            IAccessGuard companyAccessGuard,
            IOptions<CompanyOptions> companyOptions)
        {
            this.companyFAQCategoryRepository = companyFAQCategoryRepository;
            this.companyAccessGuard = companyAccessGuard;
            _companyOptions = companyOptions.Value;
        }

        public async Task<Result> Create(int routeCompanyId, CompanyFAQCategoryCreateRequest request)
        {
            var accessError = await companyAccessGuard.EnsureAccessToCompany(routeCompanyId);
            if (accessError != Error.None)
                return Result.Failure(accessError);

            var existingCount = await companyFAQCategoryRepository.Count(routeCompanyId);
            if (existingCount >= _companyOptions.MaxFAQCategories)
            {
                return Result.Failure(CompanyResults.MaxFAQCategoriesReached);
            }

            var entity = request.MapToEntity();
            entity.CompanyID = routeCompanyId;

            await companyFAQCategoryRepository.Add(entity);

            return Result.Success(CompanyResults.FAQCreated);
        }

        public async Task<Result> Delete(int routeCompanyId, int id)
        {
            var accessError = await companyAccessGuard.EnsureAccessToCompany(routeCompanyId);
            if (accessError != Error.None)
                return Result.Failure(accessError);

            var category = await companyFAQCategoryRepository.Get(id);
            if (category == null || category.CompanyID != routeCompanyId)
                return Result.Failure(GenericResults.NotFound);

            await companyFAQCategoryRepository.Delete(category);
            return Result.Success(CompanyResults.FAQCategoryDeleted);

        }

        public async Task<Result<IEnumerable<CompanyFAQCategoryDTO>>> GetAll(int companyID)
        {
            var companyFAQCategories = await companyFAQCategoryRepository.GetAll(companyID);
            var companyFAQCategoryDTOs = companyFAQCategories.Select(c => c.MapToDTO());

            return Result.Success(companyFAQCategoryDTOs);
        }

        public async Task<Result> Update(int routeCompanyId, CompanyFAQCategoryUpdateRequest request)
        {
            var accessError = await companyAccessGuard.EnsureAccessToCompany(routeCompanyId);
            if (accessError != Error.None)
                return Result.Failure(accessError);

            var category = await companyFAQCategoryRepository.Get(request.Id);
            if (category == null || category.CompanyID != routeCompanyId)
                return Result.Failure(GenericResults.NotFound);

            request.MapToEntity(category);
            await companyFAQCategoryRepository.Update(category);

            return Result.Success(CompanyResults.FAQCategoryUpdated);
        }
    }
}
