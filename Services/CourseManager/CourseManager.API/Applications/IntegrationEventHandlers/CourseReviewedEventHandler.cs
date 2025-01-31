using CourseManager.API.Protos;
using CourseManager.Domain.Contracts;
using EventBus.Abstractions;
using EventBus.Events;
using MassTransit;

namespace CourseManager.API.Applications.IntegrationEventHandlers
{
    public class CourseReviewedEventHandler(
        LearningServiceProtoGrpc.LearningServiceProtoGrpcClient client, 
        ICourseRepository repo,
        ILogger<CourseReviewedEventHandler> logger) : IIntegrationEventHandler<CourseReviewedEvent>
    {
        public async Task Consume(ConsumeContext<CourseReviewedEvent> context)
        {
            var message = context.Message;
            logger.LogInformation($"Receive event course {message.CourseId} reviewed");
            var result = await client.GetTotalCourseReviewDataAsync(new RequestGetTotalCourseReview
            {
                CourseId = message.CourseId.ToString()
            });
            await repo.UpdateCourseReview(message.CourseId, result.ReviewCount, result.Average);
        }
    }
}
