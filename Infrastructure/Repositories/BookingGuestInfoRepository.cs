using Domain.Entities.Common;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class BookingGuestInfoRepository :BaseRepository<BookingGuestInfo>, IBookingGuestInfoRepository
    {
        public BookingGuestInfoRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
