using Auth.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Infrastructure.Repositories
{
    public class AuthRepo(ApplicationContext dbContext) : IAuthRepo
    {
        public async Task CreateUser(AuthUser user) => await dbContext.Users.AddAsync(user);
        public async Task<AuthUser?> FindUserByEmail(string email) 
            => await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task CreateRefreshToken(AuthRefreshToken refreshToken) => await dbContext.RefreshTokens.AddAsync(refreshToken);

        public async Task DeleteRefreshToken(Guid userId)
        {
            var refreshToken = await dbContext.RefreshTokens.FirstOrDefaultAsync(r => r.UserId == userId);
            if (refreshToken is not null)
            {
                dbContext.RefreshTokens.Remove(refreshToken);
            }
        }

        public async Task<int> SaveAsync() => await dbContext.SaveChangesAsync();

        public async Task<AuthRefreshToken?> FindByToken(string token)
        {
            return await dbContext.RefreshTokens.FirstOrDefaultAsync(r => r.Token == token);
        }
    }
}
