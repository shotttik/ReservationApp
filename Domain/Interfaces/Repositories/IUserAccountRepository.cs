using Domain.Entities.User;

namespace Domain.Interfaces.Repositories
{
    public interface IUserAccountRepository :IBaseRepository<UserAccount>
    {
        Task<UserAccount?> GetAuthorizationData(int ID);
        Task<UserAccount?> GetByUserLoginDataID(int userLoginDataID);
        Task<UserAccount?> GetWithEmployeeServices(int userLoginDataID);
        Task<UserAccount?> GetByUserLoginDataIDWithWorkSchedules(int userLoginDataID);
        Task<UserAccount?> GetByUserLoginDataIDWithWorkScheduleExceptions(int userLoginDataID);
        Task<UserAccount?> GetEmployeeByUserLoginDataIDWithBookingData(int userLoginDataID);
        Task<UserAccount?> GetByEmailWithClientBookingData(string email);
        Task<UserAccount?> GetByUserLoginDataIDWithBookingData(int userLoginDataID);
        Task<List<int>> GetActiveUserAccountIdsByCompanyIdAsync(int companyId, CancellationToken cancellationToken);
        Task<List<int>> GetActiveUserAccountIdsByBranchIdAsync(int branchId, CancellationToken cancellationToken);
    }
}
