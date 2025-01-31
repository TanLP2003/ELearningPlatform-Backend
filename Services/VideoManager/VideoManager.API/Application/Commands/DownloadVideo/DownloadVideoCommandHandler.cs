using Application.Messaging;
using MediatR;
using VideoManager.API.Application.DomainEvents;
using VideoManager.API.Application.Services;
using VideoManager.Domain.Contracts;
using VideoManager.Domain.ValueObject;

namespace VideoManager.API.Application.Commands.DownloadVideo;
public class DownloadVideoCommandHandler(ILocalStorageRepository localStorageRepository, IFileDownloader fileDownloader, IPublisher publisher,
    ILogger<DownloadVideoCommandHandler> logger) : ICommandHandler<DownloadVideoCommand, Unit>
{
    public async Task<Unit> Handle(DownloadVideoCommand request, CancellationToken cancellationToken)
    {
        var downloadResult = await fileDownloader.DownloadFileAsync(request.File.OpenReadStream(), request.File.FileName, request.DownloadFolder);
        if(downloadResult.IsFailure)
        {
            logger.LogError("=======> FAILED TO DOWNLOAD VIDEO");
            await publisher.Publish(new VideoDownloadFailedEvent(request.DownloadFolder), cancellationToken);
            return Unit.Value;
        }
        var downloadedPath = downloadResult.Value;
        var video = request.Video;
        var result = video.SetVideoRawPath(downloadedPath);
        if(result.IsFailure)
        {
            logger.LogError("=======> FAILED TO DOWNLOAD VIDEO");
            await publisher.Publish(new VideoDownloadFailedEvent(request.DownloadFolder), cancellationToken);
            return Unit.Value;
        }
        await publisher.Publish(new VideoDownloadSuccessedEvent(request.Video, downloadedPath, request.DownloadFolder, request.LectureId), cancellationToken);
        logger.LogInformation("========> SUCCESS DOWNLOAD VIDEO");  
        return Unit.Value;
    }
}