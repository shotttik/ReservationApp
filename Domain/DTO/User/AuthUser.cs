using Domain.Enums;
using System.Text.Json.Serialization;

namespace Domain.DTO.User
{
    public class AuthUser :UserAccountDTO
    {
        public override int Id { get; set; } // UserLoginDataID
        [JsonIgnore]
        public int UserAccountId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public ActiveStatus ActiveStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}