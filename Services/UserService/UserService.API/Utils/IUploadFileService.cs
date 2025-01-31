namespace UserService.API.Utils
{
    public interface IUploadFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string bucketName, string? objectName);
    }
}
