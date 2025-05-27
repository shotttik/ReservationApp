namespace Domain.DTO.User
{
    public class AuthUser :UserAccountDTO
    {
        public override int ID { get; set; } // UserLoginDataID
        public string Email { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
    }
}