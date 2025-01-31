using Domain.DomainEvent;
using EventBus.Abstractions;
using EventBus.Events;
using MediatR;
using VideoManager.API.Application.Commands.RevertVideoProcess;
using VideoManager.API.Application.DomainEvents;

namespace VideoManager.API.Application.DomainEventHandlers;

public class VideoStoreSuccessedEventHandler(IEventBus bus) : IDomainEventHandler<VideoStoredSuccessedEvent>
{
    public async Task Handle(VideoStoredSuccessedEvent notification, CancellationToken cancellationToken)
    {
        var newVideoProcessedEvent = new VideoProcessedEvent
        {
            VideoId = notification.Video.Id,
            LectureId = notification.LectureId,
            UserId = notification.Video.UserId,
            VideoName = notification.Video.OriginalName,
            RawVideoPath = notification.Video.VideoRawPath,
            ProcessedVideoPath = notification.Video.VideoProcessedPath,
            VideoStatus = notification.Video.Status.ToString()
        };
        await bus.PublishEventAsync(newVideoProcessedEvent, cancellationToken);    
    }
}

public class VideoStoreFailedEventHandler(ISender sender) : IDomainEventHandler<VideoStoredFailedEvent>
{
    public async Task Handle(VideoStoredFailedEvent notification, CancellationToken cancellationToken)
    {
        await sender.Send(new RevertVideoProcessCommand(notification.DownloadedFolder), cancellationToken);
    }
}