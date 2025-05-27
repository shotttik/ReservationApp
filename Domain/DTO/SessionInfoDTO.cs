using Domain.DTO.User;

namespace Domain.DTO
{
    public class SessionInfoDTO
    {
        public string SessionID { get; set; } = default!;
        public AuthUser Authuser { get; set; } = default!;
        public DeviceInfo DeviceInfo { get; set; } = default!;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastAccessedAt { get; set; } = DateTime.UtcNow;
    }
    public class DeviceInfo
    {
        public string DeviceName { get; set; } = string.Empty;
        public string Browser { get; set; } = string.Empty;
        public string OS { get; set; } = string.Empty;
        public string IP { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
    }
}