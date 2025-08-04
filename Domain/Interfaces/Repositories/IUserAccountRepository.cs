using Domain.Entities.User;

namespace Domain.Interfaces.Repositories
{
    public interface IUserAccountRepository :IBaseRepository<UserAccount>
    {
        Task<UserAccount?> GetAuthorizationData(int ID);
        Task<UserAccount?> GetByUserLoginDataID(int userLoginDataID);
        Task<UserAccount?> GetByUserLoginDataIDWithWorkSchedules(int userLoginDataID);
        Task<UserAccount?> GetByUserLoginDataIDWithWorkScheduleExceptions(int userLoginDataID);
        Task<UserAccount?> GetByUserLoginDataIDWithBookingData(int userLoginDataID);
        Task<UserAccount?> GetByEmailWithClientBookingData(string email);
    }
}
