using EventBus.Abstractions;
using EventBus.Events;
using MassTransit;
using VideoLibrary.API.Infrastructure.Repositories;
using VideoLibrary.API.Models;

namespace VideoLibrary.API.Application.IntegrationEventHandler;

public class VideoUploadedEventHandler(IVideoLibraryRepository repo, 
    ILogger<VideoUploadedEventHandler> logger) : IIntegrationEventHandler<VideoProcessedEvent>
{
    public async Task Consume(ConsumeContext<VideoProcessedEvent> context)
    {
        var message = context.Message;
        var fileItem = new FileItem
        {
            FileId = message.VideoId,
            FileName = message.VideoName,
            Type = "Video",
            CreatedAt = DateTime.UtcNow
        };
        var userCollection = await repo.AddVideoToCollection(message.UserId, fileItem);
        await repo.CommitAsync();
    }
}