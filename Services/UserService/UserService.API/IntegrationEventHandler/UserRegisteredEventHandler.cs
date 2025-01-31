using EventBus.Abstractions;
using EventBus.Events;
using MassTransit;
using UserService.API.Infrastructure.Repositories;
using UserService.API.Models;

namespace UserService.API.IntegrationEventHandler
{
    public class UserRegisteredEventHandler(IProfileRepository repo, ILogger<UserRegisteredEventHandler> logger) : IIntegrationEventHandler<UserRegisteredEvent>
    {
        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            var message = context.Message;
            logger.LogInformation($"Receive Event: {message.GetType().ToString()} with message {message}");
            var newUserProfile = new Profile
            {
                Id = message.UserId,
                FirstName = message.FirstName,
                LastName = message.LastName,
            };
            await repo.CreateUserProfile(newUserProfile);
            var result = await repo.SaveChanges();
            if (result > 0)
            {
                logger.LogInformation("========> Create User Profile Successfull");
            }
            else logger.LogError("==========> Failed to create user profile");
        }
    }
}
