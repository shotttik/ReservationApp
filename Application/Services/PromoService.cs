using Application.Common.Requests.Promo;
using Application.Common.Results;
using Application.Extensions.Mappers;
using Application.Extensions.Mappers.Pagination;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.DTO;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    public class PromoService :IPromoService
    {
        private readonly IPromoCodeRepository _promoCodeRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IAccessGuard _accessGuard;

        public PromoService(IPromoCodeRepository promoCodeRepository,
            ICompanyRepository companyRepository,
            IServiceRepository serviceRepository,
            IAccessGuard accessGuard)
        {
            _promoCodeRepository = promoCodeRepository;
            _companyRepository = companyRepository;
            _serviceRepository = serviceRepository;
            _accessGuard = accessGuard;
        }
        public async Task<PromoResult> ApplyPromo(string code, int companyId, int serviceId)
        {
            var promo = await _promoCodeRepository.Get(code, companyId);
            var service = await _serviceRepository.Get(serviceId, companyId);

            if (promo == null || !promo.IsActive)
                return PromoResults.Invalid();

            if (service == null)
                return PromoResults.ServiceNotFound();

            if (DateTime.UtcNow < promo.ValidFrom || DateTime.UtcNow > promo.ValidTo)
                return PromoResults.Expired();

            if (promo.MaxUsage.HasValue && promo.UsedCount >= promo.MaxUsage)
                return PromoResults.LimitReached();

            if (promo.MinBookingPrice.HasValue && service.Price < promo.MinBookingPrice)
                return PromoResults.MinAmountReached();

            decimal discount = 0;

            if (promo.DiscountPercent.HasValue)
                discount = service.Price * promo.DiscountPercent.Value / 100;

            if (promo.DiscountAmount.HasValue)
                discount = promo.DiscountAmount.Value;

            return new PromoResult
            {
                IsValid = true,
                Discount = discount,
                Promo = promo
            };
        }

        // this method is only for UX endpoint to check if promo can be applied
        public async Task<Result<PromoCodeDTO>> ValidateApplyPromo(string code, int companyId, int serviceId)
        {
            var result = await ApplyPromo(code, companyId, serviceId);
            if (result.IsValid)
            {
                return Result.Success(result.Promo!.MapToDTO());
            }
            return Result.Failure<PromoCodeDTO>(result.Error);
        }

        public async Task<Result<PromoCodeDTO>> Create(int companyId, PromoCodeCreateRequest request)
        {
            var accessError = await _accessGuard.EnsureAccessToCompany(companyId);
            if (accessError != Error.None)
            {
                return Result.Failure<PromoCodeDTO>(accessError);
            }
            var company = await _companyRepository.Get(companyId);
            if (company == null)
            {
                return Result.Failure<PromoCodeDTO>
                    (CompanyResults.CompanyDoesNotExists);
            }
            if (company.IsDisabled)
            {
                return Result.Failure<PromoCodeDTO>(PromoResults.CompanyIsDisabled);
            }
            var codeExists = await _promoCodeRepository.ExistsByCode(request.Code);
            if (codeExists)
            {
                return Result.Failure<PromoCodeDTO>(PromoResults.CodeAlreadyExists);
            }
            var promo = request.MapToEntity();
            promo.CompanyId = companyId;

            await _promoCodeRepository.Add(promo);

            return promo.MapToDTO();
        }

        public async Task<Result<PromoCodeDTO>> Update(int id, int companyId, PromoCodeUpdateRequest request)
        {
            var promo = await _promoCodeRepository.Get(id, companyId);
            if (promo == null)
            {
                return Result.Failure<PromoCodeDTO>(PromoResults.NotFound);
            }
            var accessError = await _accessGuard.EnsureAccessToCompany(companyId);
            if (accessError != Error.None)
            {
                return Result.Failure<PromoCodeDTO>(accessError);
            }

            var codeExists = await _promoCodeRepository.ExistsByCode(request.Code, id);
            if (codeExists)
            {
                return Result.Failure<PromoCodeDTO>(PromoResults.CodeAlreadyExists);
            }

            promo.ApplyUpdate(request);
            await _promoCodeRepository.Update(promo);

            return promo.MapToDTO();
        }
        public async Task<Result> Delete(int id, int companyId)
        {
            var promo = await _promoCodeRepository.Get(id);

            if (promo == null || promo.CompanyId != companyId)
                return Result.Failure(PromoResults.NotFound);

            await _promoCodeRepository.Delete(promo);

            return Result.Success();
        }
        public async Task<Result<PagedList<PromoCodeDTO>>> RetrievePaged(int routeCompanyId, PagedParameters parameters, CancellationToken cancellationToken)
        {
            var accessError = await _accessGuard.EnsureAccessToCompany(routeCompanyId);
            if (accessError != Error.None)
            {
                return Result.Failure<PagedList<PromoCodeDTO>>(accessError);
            }

            var allowedFields = PromoCodeFieldMap.DtoToEntityPath;
            var errors = parameters.Validate(allowedFields, typeof(PromoCodeDTO));
            if (errors.Any())
            {
                return Result.Failure<PagedList<PromoCodeDTO>>(PagedListResults.InvalidPagedParameters(errors.First()));
            }
            var users = await _promoCodeRepository.RetrievePaged(parameters, cancellationToken, routeCompanyId);

            return Result.Success(users);
        }
    }
}
