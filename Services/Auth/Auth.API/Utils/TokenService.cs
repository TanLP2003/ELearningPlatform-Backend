
using Auth.API.Infrastructure.Repositories;
using Auth.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Auth.API.Utils
{
    public class TokenService(IConfiguration config, IAuthRepo repo) : ITokenService
    {
        public string GenerateAccessToken(Guid userId)
        {
            var secretKey = config.GetSection("Security")["SecretKey"];
            var encodedKey = Encoding.UTF8.GetBytes(secretKey);
            var symmetricKey = new SymmetricSecurityKey(encodedKey);
            var claims = new Claim[] {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
            var tokenOpt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(10),
                signingCredentials: new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256)
            );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOpt);
            return accessToken;
        }

        public async Task<AuthRefreshToken> GenerateRefreshToken(Guid userId)
        {
            await repo.DeleteRefreshToken(userId);
            var randomByte = new byte[32];
            string token;
            using (var randomGenerator = RandomNumberGenerator.Create())
            {
                randomGenerator.GetBytes(randomByte);
                token = Convert.ToBase64String(randomByte);
            }
            var refreshToken = new AuthRefreshToken
            {
                UserId = userId,
                Token = token,
                ExpiredAt = DateTime.UtcNow.AddDays(10)
            };
            await repo.CreateRefreshToken(refreshToken);
            return refreshToken;
        }
    }
}
