using UserService.API.Models;

namespace UserService.API.Infrastructure.Repositories
{
    public interface IProfileRepository
    {
        Task<int> SaveChanges();
        Task<Profile?> GetUserProfileById(Guid id);
        Task CreateUserProfile(Profile profile);
    }
}
