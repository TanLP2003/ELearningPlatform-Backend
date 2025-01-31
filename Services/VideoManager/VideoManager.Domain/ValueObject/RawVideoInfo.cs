using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoManager.Domain.ValueObject;

public record RawVideoInfo
{
    public string VideoName { get; set; }
    public string DownloadedFolder { get; set; }
    public string RawVideoPath { get; set; }

    public RawVideoInfo(string videoName, string downloadedFolder, string rawVideoPath)
    {
        VideoName = videoName;
        DownloadedFolder = downloadedFolder;
        RawVideoPath = rawVideoPath;    
    }
}