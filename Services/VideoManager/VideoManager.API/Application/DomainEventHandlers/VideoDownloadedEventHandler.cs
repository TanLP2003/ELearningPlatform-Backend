using Domain.DomainEvent;
using MediatR;
using VideoManager.API.Application.Commands.ProcessVideo;
using VideoManager.API.Application.DomainEvents;
using VideoManager.API.Application.Services;

namespace VideoManager.API.Application.DomainEventHandlers;

public class VideoDownloadSuccessedEventHandler(ISender sender) : IDomainEventHandler<VideoDownloadSuccessedEvent>
{
    public async Task Handle(VideoDownloadSuccessedEvent notification, CancellationToken cancellationToken)
    {
        var convertVideoCommand = new ConvertVideoCommand(notification.Video, notification.DownloadedPath, notification.DownloadedFolder, notification.LectureId);
        await sender.Send(convertVideoCommand, cancellationToken);
    }
}

public class VideoDownLoadFailedEventHandler(ISender sender) : IDomainEventHandler<VideoDownloadFailedEvent>
{
    public async Task Handle(VideoDownloadFailedEvent notification, CancellationToken cancellationToken)
    {
        await sender.Send(notification.DownloadedFolder, cancellationToken);
    }
}