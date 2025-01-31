using Domain;

namespace VideoManager.API.Application.Services;

public interface IFileDownloader
{
    Task<Result<string>> DownloadFileAsync(Stream fileStream, string videoName, string fileContainedFolder);
}