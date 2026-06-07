using Application.Common.Requests.Promo;
using Application.Common.Results;
using Domain.Abstractions;
using Domain.DTO;

namespace Application.Interfaces
{
    public interface IPromoService
    {
        Task<PromoResult> ApplyPromo(string code, int companyId, int serviceId);
        Task<Result<PromoCodeDTO>> ValidateApplyPromo(string code, int companyId, int serviceId);
        Task<Result<PromoCodeDTO>> Create(int companyId, PromoCodeCreateRequest request);
        Task<Result<PromoCodeDTO>> Update(int id, int companyId, PromoCodeUpdateRequest request);
        Task<Result> Delete(int id, int companyId);
        Task<Result<PagedList<PromoCodeDTO>>> RetrievePaged(int routeCompanyId, PagedParameters parameters, CancellationToken cancellationToken);
    }
}
