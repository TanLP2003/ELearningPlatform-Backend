using EventBus.Abstractions;
using EventBus.Events;
using LearningService.API.Entities;
using LearningService.API.Infrastructure.Repositories;
using MassTransit;
using Newtonsoft.Json;

namespace LearningService.API.Applications.IntegrationEventHandler
{
    public class LearningServiceOrderCreatedEventHandler(
        ILearningRepo repo,
        ILogger<LearningServiceOrderCreatedEventHandler> logger
        ) : IIntegrationEventHandler<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var message = context.Message;
            logger.LogInformation($"Receive Event: {JsonConvert.SerializeObject(message, Formatting.Indented)}");
            var newEnrolledCourses = new List<EnrolledCourse>();
            foreach (var item in message.Items)
            {
                var enrolledCourse = await repo.GetEnrolledCourseByUserIdAndCourseId(message.UserId, item.CourseId);
                if (enrolledCourse is null)
                {
                    enrolledCourse = new EnrolledCourse
                    {
                        UserId = message.UserId,
                        CourseId = item.CourseId,
                        EnrollmentDate = DateTime.UtcNow,
                        CompletionPercentage = 0
                    };
                    newEnrolledCourses.Add(enrolledCourse);
                }
            }
            await repo.AddManyEnrolledCourses(newEnrolledCourses);
        }
    }
}
