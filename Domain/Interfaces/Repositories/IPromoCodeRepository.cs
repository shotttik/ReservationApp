using Domain.Abstractions;
using Domain.DTO;
using Domain.DTO.User;
using Domain.Entities.Common;

namespace Domain.Interfaces.Repositories
{
    public interface IPromoCodeRepository :IBaseRepository<PromoCode>
    {
        Task<PromoCode?> Get(string code, int companyId);
        Task<PromoCode?> Get(int id, int companyId);
        Task<bool> ExistsByCode(string code);
        Task<bool> ExistsByCode(string code, int id);
        Task<PagedList<PromoCodeDTO>> RetrievePaged(
         PagedParameters parameters,
         CancellationToken cancellationToken,
         int companyID);
    }
}
