using Domain.Entities.Common;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class BookingVerificationRepository :BaseRepository<BookingVerification>, IBookingVerificationRepository
    {
        public BookingVerificationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}
