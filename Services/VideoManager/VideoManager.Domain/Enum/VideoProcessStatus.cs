using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoManager.Domain.Enum;

public enum VideoProcessStatus
{
    Started = 0,
    //Downloading = 1,
    Downloaded = 1,
    //ProcessingVideos = 3,
    Processed = 2,
    Failed = 3
}