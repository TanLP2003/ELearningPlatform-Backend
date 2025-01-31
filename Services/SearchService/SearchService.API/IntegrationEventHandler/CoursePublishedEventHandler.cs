using EventBus.Abstractions;
using EventBus.Events;
using MassTransit;
using SearchService.API.Entities;
using SearchService.API.Infrastructure.Repositories;

namespace SearchService.API.IntegrationEventHandler
{
    public class CoursePublishedEventHandler(ISearchRepo repo, ILogger<CoursePublishedEventHandler> logger) : IIntegrationEventHandler<CoursePublishedEvent>
    {
        public async Task Consume(ConsumeContext<CoursePublishedEvent> context)
        {
            var message = context.Message;
            logger.LogInformation($"Receive message: {message}");
            var searchCourse = new SearchCourse
            {
                CourseId = message.CourseId,
                CourseTitle = message.CourseTitle,
                CourseImage = message.CourseImage,
                InstructorName = message.InstructorName
            };
            await repo.CreateSearchCourse(searchCourse);
        }
    }
}
