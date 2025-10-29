using System.Text.Json.Serialization;

namespace Domain.DTO.User
{
    public class AuthUser :UserAccountDTO
    {
        public override int ID { get; set; } // UserLoginDataID
        [JsonIgnore]
        public int UserAccountId { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
    }
}