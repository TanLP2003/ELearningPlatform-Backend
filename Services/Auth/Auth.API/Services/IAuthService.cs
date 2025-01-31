using Auth.API.Models;
using Domain;

namespace Auth.API.Services
{
    public interface IAuthService
    {
        Task<Result<AuthUser>> FindUserByEmail(string email);
        Task<Result<AuthRefreshToken>> FindByToken(string email);
        Task<Result<AuthUser>> CreateUserAsync(AuthUser user, string firstName, string lastName);
    }
}
