using Domain.DomainEvent;
using MediatR;
using VideoManager.API.Application.Commands.RevertVideoProcess;
using VideoManager.API.Application.Commands.StoreVideo;
using VideoManager.API.Application.DomainEvents;

namespace VideoManager.API.Application.DomainEventHandlers;

public class VideoConvertSuccessedEventHandler(ISender sender) : IDomainEventHandler<VideoConvertSuccessedEvent>
{
    public async Task Handle(VideoConvertSuccessedEvent notification, CancellationToken cancellationToken)
    {
        await sender.Send(new StoreVideoCommand(notification.Video, notification.LectureId), cancellationToken);
    }
}

public class VideoConvertFailedEventHandler(ISender sender) : IDomainEventHandler<VideoConvertFailedEvent>
{
    public async Task Handle(VideoConvertFailedEvent notification, CancellationToken cancellationToken)
    {
        await sender.Send(new RevertVideoProcessCommand(notification.DownloadedFolder), cancellationToken);
    }
}