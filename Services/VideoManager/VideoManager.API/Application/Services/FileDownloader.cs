
using Domain;
using VideoManager.Domain.Entities;

namespace VideoManager.API.Application.Services;

public class FileDownloader : IFileDownloader
{
    public async Task<Result<string>> DownloadFileAsync(Stream fileStream, string videoName, string fileContainedFolder)
    {
        try
        {
            var videoDownloadedPath = Path.Combine(fileContainedFolder, videoName);
            using var stream = File.Create(videoDownloadedPath);
            await fileStream.CopyToAsync(stream);
            return videoDownloadedPath;
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(VideoError.VideoDownloadFailed);
        }
    }
}