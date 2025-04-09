using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICompanyInvitationRepository :IBaseRepository<CompanyInvitation>
    {
        Task<CompanyInvitation?> Get(string token);
        Task RevokePreviousInvite(int userAccountID);
    }
}
