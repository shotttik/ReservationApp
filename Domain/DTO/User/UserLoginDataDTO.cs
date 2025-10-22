using Domain.Enums;

namespace Domain.DTO.User
{
    public class UserLoginDataDTO :UserAccountDTO
    {
        public override int ID { get; set; }
        public string Email { get; set; } = string.Empty;
        public VerificationStatus VerificationStatus { get; set; }
        public ActiveStatus ActiveStatus { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
