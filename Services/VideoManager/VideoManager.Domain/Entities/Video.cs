using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoManager.Domain.Enum;

namespace VideoManager.Domain.Entities;

public class Video : Entity<Guid>
{
    private Video(Guid id, Guid userId, string originalName) : base(id)
    {
        UserId = userId; 
        OriginalName = originalName;
        Status = VideoProcessStatus.Started;
    }
    public Guid UserId { get; private set; }
    public string OriginalName { get; private set; }
    public string? VideoRawPath { get; private set; }
    public string? VideoProcessedPath { get ; private set; }
    public VideoProcessStatus Status { get; private set; }
    public static Video Create(Guid userId, string originalName)
    {
        return new(Guid.NewGuid(), userId, originalName);
    }
    public Result SetVideoRawPath(string downloadedPath)
    {
        if(Status != VideoProcessStatus.Started)
        {
            return Result.Failure(VideoError.ProcessNotStarted);
        }
        VideoRawPath = downloadedPath;
        Status = VideoProcessStatus.Downloaded;
        return Result.Success();
    }

    public Result SetVideoProcessPath(string processPath)
    {
        if(Status < VideoProcessStatus.Downloaded)
        {
            return Result.Failure(VideoError.VideoNotDownloaded);
        }
        VideoProcessedPath = processPath;
        Status = VideoProcessStatus.Processed;
        return Result.Success();
    }
}
