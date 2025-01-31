using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoManager.Domain.Entities;

public class VideoError
{
    private static string code = "Error.Video";
    public static Error ProcessNotStarted => Error.Create(code, "Process is not started yet or already downloaded!");
    public static Error VideoNotDownloaded => Error.Create(code, "Video is not downloaded yet!");
    public static Error VideoDownloadFailed => Error.Create(code, "Download video failed!");
    public static Error VideoConvertFailed => Error.Create(code, "Convert video failed!");
}