using Application.Authentication;
using Domain.DTO;
using Domain.DTO.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shared.Utilities;

namespace Application.Helpers
{
    internal static class SessionHelper
    {
        public static SessionInfoDTO BuildSessionInfo(HttpContext context, IConfiguration configuration, AuthUser user)
        {
            var userAgent = context.Request.Headers ["User-Agent"].ToString();
            var ip = context.Connection.RemoteIpAddress?.ToString();
            var userAgentInfo = UserAgentUtils.Parse(userAgent);

            var deviceInfo = new DeviceInfo
            {
                DeviceName = userAgentInfo.Device,
                Browser = userAgentInfo.Browser,
                OS = userAgentInfo.OperatingSystem,
                IP = ip ?? "",
                UserAgent = userAgent
            };

            var refreshToken = JWTGenerator.GenerateAndHashSecureToken();
            var refreshTokenExpTime = DateTime.UtcNow.AddDays(Convert.ToDouble(configuration ["Jwt:RefreshTokenExpirationDays"]));

            return new SessionInfoDTO
            {
                SessionID = Guid.NewGuid().ToString(),
                AuthUser = user,
                DeviceInfo = deviceInfo,
                RefreshToken = refreshToken,
                RefreshTokenExpTime = refreshTokenExpTime,
                CreatedAt = DateTime.UtcNow,
                LastAccessedAt = DateTime.UtcNow
            };
        }
    }
}