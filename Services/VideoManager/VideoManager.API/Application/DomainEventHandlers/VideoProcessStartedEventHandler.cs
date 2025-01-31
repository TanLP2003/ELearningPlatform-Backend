using Domain.DomainEvent;
using MediatR;
using VideoManager.API.Application.Commands.DownloadVideo;
using VideoManager.API.Application.DomainEvents;

namespace VideoManager.API.Application.DomainEventHandlers;

public class VideoProcessStartedEventHandler(ISender sender) : IDomainEventHandler<VideoProcessStartedEvent>
{
    public async Task Handle(VideoProcessStartedEvent notification, CancellationToken cancellationToken)
    {
        var downloadVideoCommand = new DownloadVideoCommand(notification.Video, notification.DownloadFolder, notification.File, notification.LectureId);
        await sender.Send(downloadVideoCommand, cancellationToken);
    }
}