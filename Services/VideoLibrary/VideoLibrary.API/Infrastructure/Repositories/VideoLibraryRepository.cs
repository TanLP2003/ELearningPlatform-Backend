using Microsoft.EntityFrameworkCore;
using VideoLibrary.API.Models;

namespace VideoLibrary.API.Infrastructure.Repositories;

public class VideoLibraryRepository(VideoLibraryContext dbContext) : IVideoLibraryRepository
{
    public async Task<FileList> AddVideoToCollection(Guid userId, FileItem item)
    {
        var userCollection = await dbContext.FileLists.FirstOrDefaultAsync(fl => fl.UserId == userId);
        if (userCollection == null)
        {
            userCollection = new FileList
            {
                UserId = userId
            };
            await dbContext.FileLists.AddAsync(userCollection);
        }
        userCollection.Items.Add(item);
        return userCollection;
    }

    public async Task<int> CommitAsync()
    {
        return await dbContext.SaveChangesAsync();
    }

    public async Task<FileList> GetFileCollectionByUser(Guid userId)
    {
        var userCollection = await dbContext.FileLists.FirstOrDefaultAsync(fl => fl.UserId == userId);
        if (userCollection == null)
        {
            userCollection = new FileList
            {
                UserId = userId
            };
            await dbContext.FileLists.AddAsync(userCollection);
            await dbContext.SaveChangesAsync(); 
        }
        return userCollection;
    }
}