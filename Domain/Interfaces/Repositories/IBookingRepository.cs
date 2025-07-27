using Domain.Entities.Common;

namespace Domain.Interfaces.Repositories
{
    public interface IBookingRepository :IBaseRepository<Booking>
    {
        Task<bool> HasBookingOverlap(int userId, DateTime start, DateTime end, bool asEmployee);
    }
}
