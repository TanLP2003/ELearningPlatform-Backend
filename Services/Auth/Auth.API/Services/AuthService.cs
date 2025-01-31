using Auth.API.Infrastructure.Repositories;
using Auth.API.Models;
using Domain;
using EventBus.Abstractions;
using EventBus.Events;

namespace Auth.API.Services
{
    public class AuthService(IAuthRepo repo, IEventBus eventBus) : IAuthService
    {
        public async Task<Result<AuthRefreshToken>> FindByToken(string token)
        {
            var refreshToken = await repo.FindByToken(token);
            if (refreshToken == null) return Result.Failure<AuthRefreshToken>(Error.Create("RefreshToken.NotFound", $"Refresh token: {token} is not exist"));
            return refreshToken;
        }

        public async Task<Result<AuthUser>> FindUserByEmail(string email)
        {
            var user = await repo.FindUserByEmail(email);
            if (user is null) return Result.Failure<AuthUser>(Error.Create("User.NotFound", $"User with email: {email} is not exist"));
            return user;
        }

        public async Task<Result<AuthUser>> CreateUserAsync(AuthUser user, string firstName, string lastName)
        {
            await repo.CreateUser(user);
            var success = await repo.SaveAsync();
            if (success > 0)
            {
                var @event = new UserRegisteredEvent
                {
                    UserId = user.Id,
                    FirstName = firstName,
                    LastName = lastName
                };
                await eventBus.PublishEventAsync(@event);
                return user;
            }
            else return Result.Failure<AuthUser>(Error.Create("User.CreateError", "Can not create user"));
        }
    }
}
