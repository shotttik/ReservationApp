using Domain.DTO;
using Domain.Entities.Common;

namespace Application.Extensions.Mappers
{
    public static class GuestInfoMapper
    {
        public static BookingGuestInfoDTO MapToDTO(this BookingGuestInfo guestInfo)
        {
            return new BookingGuestInfoDTO
            {
                Id = guestInfo.Id,
                BookingId = guestInfo.BookingId,
                ContactType = guestInfo.ContactType,
                Contact = guestInfo.Contact,
                DisplayName = guestInfo.DisplayName
            };
        }
    }
}
