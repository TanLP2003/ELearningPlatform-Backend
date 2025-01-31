using Microsoft.EntityFrameworkCore;
using UserService.API.Models;

namespace UserService.API.Infrastructure.Repositories
{
    public class ProfileRepository(ApplicationContext dbContext) : IProfileRepository
    {
        public async Task CreateUserProfile(Profile profile) => await dbContext.Profiles.AddAsync(profile);
        public async Task<int> SaveChanges() => await dbContext.SaveChangesAsync();

        public async Task<Profile?> GetUserProfileById(Guid id) => await dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == id);
    }
}
