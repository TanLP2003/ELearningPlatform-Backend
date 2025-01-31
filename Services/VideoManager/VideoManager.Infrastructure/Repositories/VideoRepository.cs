using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoManager.Domain.Contracts;
using VideoManager.Domain.Entities;

namespace VideoManager.Infrastructure.Repositories;

public class VideoRepository(VideoManagerContext dbContext) : IVideoRepository
{
    public async Task<Video> GetVideoById(Guid id, Guid userId)
    {
        var video = await dbContext.Videos.FirstOrDefaultAsync(v => v.Id == id && v.UserId == userId);
        return video;
    }
}