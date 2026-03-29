using Domain.Entities.Common;
using Domain.Entities.User;

namespace Domain.Entities.CompanyReleated
{
    public class EmployeeService :BaseEntity
    {
        public int EmployeeId { get; set; }
        public UserAccount Employee { get; set; } = null!;

        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;

    }
}
