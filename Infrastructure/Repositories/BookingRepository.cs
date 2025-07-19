using Domain.Entities.Common;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class BookingRepository :BaseRepository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}
