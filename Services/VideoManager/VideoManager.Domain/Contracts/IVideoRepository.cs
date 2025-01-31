using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoManager.Domain.Entities;

namespace VideoManager.Domain.Contracts;

public interface IVideoRepository
{
    Task<Video> GetVideoById(Guid id, Guid userId);
}
