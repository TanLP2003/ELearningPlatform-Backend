
using Minio;

namespace UserService.API.Utils
{
    public class UploadFileService(MinioClient client, ILogger<UploadFileService> logger) : IUploadFileService
    {

        public Task<string> UploadFileAsync(IFormFile file, string bucketName, string? objectName)
        {
            throw new NotImplementedException();
        }
    }
}
