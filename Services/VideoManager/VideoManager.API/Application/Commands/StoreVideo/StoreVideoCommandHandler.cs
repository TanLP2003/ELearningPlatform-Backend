using Application.Messaging;
using MediatR;
using VideoManager.API.Application.DomainEvents;
using VideoManager.Infrastructure;

namespace VideoManager.API.Application.Commands.StoreVideo;

public class StoreVideoCommandHandler(VideoManagerContext dbContext, IPublisher publisher) : ICommandHandler<StoreVideoCommand>
{
    public async Task<Unit> Handle(StoreVideoCommand request, CancellationToken cancellationToken)
    {
        await dbContext.Videos.AddAsync(request.Video);
        var result = await dbContext.SaveChangesAsync();
        if(result == 0)
        {
            await publisher.Publish(new VideoStoredFailedEvent(request.Video.VideoRawPath!));
            return Unit.Value;
        }
        await publisher.Publish(new VideoStoredSuccessedEvent(request.Video, request.LectureId));
        return Unit.Value;
    }
}