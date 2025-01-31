using CourseManager.Domain.Contracts;
using EventBus.Abstractions;
using EventBus.Events;
using MassTransit;

namespace CourseManager.API.Applications.IntegrationEventHandlers;

public class VideoProcessedEventHandler(ICourseRepository repo, ILogger<VideoProcessedEventHandler> logger) : IIntegrationEventHandler<VideoProcessedEvent>
{
    public async Task Consume(ConsumeContext<VideoProcessedEvent> context)
    {
        var message = context.Message;
        Console.WriteLine("Message Received");
        Console.WriteLine($"{message}");
        if (message == null)
        {
            logger.LogInformation("Message is null");
        }
        logger.LogInformation($"Receive Event: {message}");
        var lecture = await repo.GetLectureById(message.LectureId);
        lecture.LectureContentUrl = message.ProcessedVideoPath;
        lecture.VideoName = message.VideoName;
        await repo.SaveChangeAsync();
    }
}