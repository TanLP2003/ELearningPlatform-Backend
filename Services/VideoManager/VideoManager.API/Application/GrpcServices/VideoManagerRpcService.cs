using Grpc.Core;
using VideoManager.API.Protos;
using VideoManager.Domain.Contracts;

namespace VideoManager.API.Application.GrpcServices
{
    public class VideoManagerRpcService(
        ILogger<VideoManagerRpcService> logger,
        IVideoRepository repo) : VideoManagerProtoService.VideoManagerProtoServiceBase
    {
        public override async Task<VideoInfo> GetVideoInfo(GetVideoInfoRequest request, ServerCallContext context)
        {
            logger.LogInformation($"Receive gRPC request for id: {request.VideoId}, userId: {request.UserId}");
            var result = await repo.GetVideoById(Guid.Parse(request.VideoId), Guid.Parse(request.UserId));
            var videoInfo = new VideoInfo
            {
                VideoId = result.Id.ToString(),
                VideoName = result.OriginalName,
                VideoRawPath = result.VideoRawPath,
                VideoProcessedPath = result.VideoProcessedPath
            };
            return videoInfo;
        }
    }
}
