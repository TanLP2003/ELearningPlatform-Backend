using Auth.API.Models;

namespace Auth.API.Infrastructure.Repositories
{
    public interface IAuthRepo
    {
        Task CreateUser(AuthUser user);
        Task<AuthUser?> FindUserByEmail(string email);
        Task<AuthRefreshToken?> FindByToken(string token);

        Task CreateRefreshToken(AuthRefreshToken token);
        Task DeleteRefreshToken(Guid userId);
        Task<int> SaveAsync();
    }
}
