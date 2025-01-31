using Auth.API.Models;

namespace Auth.API.Utils
{
    public interface ITokenService
    {
        string GenerateAccessToken(Guid userId);
        Task<AuthRefreshToken> GenerateRefreshToken(Guid userId);
    }
}
