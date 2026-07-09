using Application.Authentication;
using Application.Options;
using Domain.DTO;
using Domain.DTO.User;
using Microsoft.AspNetCore.Http;
using Shared.Utilities;

namespace Application.Helpers
{
    internal static class SessionHelper
    {
        public static SessionInfoDTO BuildSessionInfo(HttpContext context, AuthUser user, JwtOptions jwtOptions)
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
            var refreshTokenExpTime = DateTime.UtcNow.AddDays(Convert.ToDouble(jwtOptions.RefreshTokenExpirationDays));

            return new SessionInfoDTO
            {
                SessionId = Guid.NewGuid().ToString(),
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