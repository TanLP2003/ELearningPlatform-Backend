using VideoLibrary.API.Infrastructure;
using VideoLibrary.API.Infrastructure.Repositories;
using VideoLibrary.API.Models;

namespace VideoLibrary.API.Application.Services;

public class VideoLibraryService(IVideoLibraryRepository repo) : IVideoLibraryService
{
    public async Task<FileList> GetFileCollectionByUser(Guid userId)
    {
        return await repo.GetFileCollectionByUser(userId);
    }
}