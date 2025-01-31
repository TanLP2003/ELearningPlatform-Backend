using VideoLibrary.API.Models;

namespace VideoLibrary.API.Infrastructure.Repositories;

public interface IVideoLibraryRepository
{
    Task<FileList> GetFileCollectionByUser(Guid userId);
    Task<int> CommitAsync();
    Task<FileList> AddVideoToCollection(Guid userId, FileItem item);
}