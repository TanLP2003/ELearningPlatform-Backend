using EventBus.Abstractions;
using EventBus.Events;
using MassTransit;
using SearchService.API.Entities;
using SearchService.API.Infrastructure.Repositories;

namespace SearchService.API.IntegrationEventHandler
{
    public class UserRegisterEventHandler(ILogger<UserRegisterEventHandler> logger, ISearchRepo repo) : IIntegrationEventHandler<UserRegisteredEvent>
    {
        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            var message = context.Message;
            logger.LogInformation($"Search Service receive message: {message}");
            var searchInstructor = new SearchInstructor
            {
                InstructorId = message.UserId,
                InstructorName = message.FirstName + message.LastName,
                SearchCount = 0
            };
            await repo.CreateSearchInstructor(searchInstructor);
        }
    }
}
