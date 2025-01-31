using Application.Messaging;
using MediatR;
using VideoManager.API.Application.DomainEvents;
using VideoManager.API.Application.Services;

namespace VideoManager.API.Application.Commands.ProcessVideo;

public class ConvertVideoCommandHandler(IPublisher publisher, ILogger<ConvertVideoCommandHandler> logger) : ICommandHandler<ConvertVideoCommand>
{
    public async Task<Unit> Handle(ConvertVideoCommand request, CancellationToken cancellationToken)
    {
        var rawVideoPath = request.VideoDownloadedPath;
        var folder = request.DownloadedFolder;
        //var args = FFMPEG.BuildArgs(rawVideoPath, folder, 1080, 720);
        var configs = new List<BitrateConfig>
        {
            new BitrateConfig { Resolution = "1920x1080", Bitrate = "5000k", OutputFolder = "1080p", OutputFile = "1080p.m3u8" },
            //new BitrateConfig { Resolution = "1280x720", Bitrate = "3000k", OutputFolder = "720p", OutputFile = "720p.m3u8" },
            //new BitrateConfig { Resolution = "854x480", Bitrate = "1500k", OutputFolder = "480p", OutputFile = "480p.m3u8" },
            //new BitrateConfig { Resolution = "426x240", Bitrate = "800k", OutputFolder = "240p", OutputFile = "240p.m3u8" }
        };
        var processResult = FFMPEG.CreateHLSStream(rawVideoPath, folder, configs);
        if (processResult.IsFailure)
        {
            logger.LogError("========> FAILED TO CONVERT VIDEO");
            await publisher.Publish(new VideoConvertFailedEvent(request.DownloadedFolder), cancellationToken);
            return Unit.Value;
        }
        var masterFileName = processResult.Value;
        var processedPath = Path.Combine(request.DownloadedFolder, masterFileName);
        var result = request.Video.SetVideoProcessPath(Path.GetRelativePath("/app", processedPath));
        if (result.IsFailure)
        {
            logger.LogError("========> FAILED TO CONVERT VIDEO");
            await publisher.Publish(new VideoConvertFailedEvent(request.DownloadedFolder), cancellationToken);
            return Unit.Value;
        }
        await publisher.Publish(new VideoConvertSuccessedEvent(request.Video, request.LectureId), cancellationToken);
        logger.LogInformation("=======> SUCCESS CONVERT VIDEO");
        return Unit.Value;
    }
}