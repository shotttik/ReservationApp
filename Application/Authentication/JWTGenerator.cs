using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Authentication
{
    public static class JWTGenerator
    {
        public static string GenerateAccessToken(int userLoginDataID, int userAccountID, string email, IConfiguration configuration)
        {
            var claims = new []
            {
                new Claim(ClaimTypes.PrimarySid, userLoginDataID.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Sid, userAccountID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // unique identifier for the token
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration ["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration ["Jwt:Issuer"],
                audience: configuration ["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(configuration ["Jwt:AccessTokenExpirationMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GenerateAndHashSecureToken()
        {
            var randomNumber = new byte [64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var secureToken = Convert.ToBase64String(randomNumber);

            return HashToken(secureToken);
        }

        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration configuration)
        {
            var tokenValidationParameteres = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration ["Jwt:Key"]!)),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameteres, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public static string HashToken(string token)
        {
            using var sha256 = SHA256.Create();
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var hashedBytes = sha256.ComputeHash(tokenBytes);

            // Use URL-safe Base64 encoding for the hash
            return Convert.ToBase64String(hashedBytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');
        }
    }
}
