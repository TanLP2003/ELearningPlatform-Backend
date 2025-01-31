using VideoLibrary.API.Models;

namespace VideoLibrary.API.Application.Services;

public interface IVideoLibraryService
{
    Task<FileList> GetFileCollectionByUser(Guid userId);
}