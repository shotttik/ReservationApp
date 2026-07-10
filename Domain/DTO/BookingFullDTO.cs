using Domain.DTO.Branch;
using Domain.DTO.Company;
using Domain.DTO.User;

namespace Domain.DTO
{
    public class BookingFullDTO :BookingDTO
    {
        public UserAccountDTO? Client { get; set; }
        public UserAccountDTO? Employee { get; set; }
        public BranchDTO? Branch { get; set; }
        public ServiceDTO? Service { get; set; }
        public CompanyDTO? Company { get; set; }
        public BookingGuestInfoDTO? GuestInfo { get; set; }
    }
}
