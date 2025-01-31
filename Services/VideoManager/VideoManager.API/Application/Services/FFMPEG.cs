using Domain;
using System.Diagnostics;
using System.Text;
using VideoManager.Domain.Entities;

namespace VideoManager.API.Application.Services;

public static class FFMPEG
{
    public static string BuildArgs(string rawVideoPath, string processedVideoPath, int width, int height)
    {
        var args = new StringBuilder();
        args.Append("-y ");
        args.Append($"-i \"{rawVideoPath}\" ");
        args.Append($"-s {width}x{height} ");
        args.Append($"-c:v libx264 ");
        args.Append("-b:v 2000k ");
        args.Append("-c:a aac ");
        args.Append("-master_pl_name master.m3u8 ");
        args.Append("-f hls -hls_time 10 -hls_list_size 0 ");
        args.Append($"-hls_segment_filename \"{processedVideoPath}/fileSequenced%d.ts\" ");
        args.Append($"\"{processedVideoPath}/prog_index.m3u8\"");

        return args.ToString();
    }
    public static async Task<Result<string>> RunCommand(string? args)
    {
        try
        {
            ProcessStartInfo startInfo = new()
            {
                FileName = "ffmpeg",
                Arguments = args,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            var proc = Process.Start(startInfo);
            ArgumentNullException.ThrowIfNull(proc);
            string output = proc.StandardOutput.ReadToEnd();
            await proc.WaitForExitAsync();

            return Result.Success("master.m3u8");
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(VideoError.VideoConvertFailed);
        }
    }

    public static Result<string> CreateHLSStream(string inputFile, string outputBaseFolder, List<BitrateConfig> configs)
    {
        try
        {
            //Directory.CreateDirectory(outputBaseFolder);

            Parallel.ForEach(configs, new ParallelOptions { MaxDegreeOfParallelism = 12 },
                config => ProcessBitrate(inputFile, outputBaseFolder, config));
            return CreateMasterPlaylist(outputBaseFolder, configs);
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(VideoError.VideoConvertFailed);
        }
    }
    private static void ProcessBitrate(string inputFile, string outputBaseFolder, BitrateConfig config)
    {
        try
        {
            var outputFolder = Path.Combine(outputBaseFolder, config.OutputFolder);
            Directory.CreateDirectory(outputFolder);
            var outputFile = Path.Combine(outputFolder, config.OutputFile);
            var segmentFile = Path.Combine(outputFolder, $"{config.OutputFolder}_%03d.ts");
            var arguments = $"-i \"{inputFile}\" -vf scale={config.Resolution} -c:v libx264 -b:v {config.Bitrate} " +
                        $"-c:a aac -b:a 128k -hls_time 8 -hls_list_size 0 -hls_segment_filename " +
                        $"\"{segmentFile}\" \"{outputFile}\"";

            Console.WriteLine($"==========> Processing {config.Resolution} with bitrate {config.Bitrate}");
            Console.WriteLine($"==========> Command: ffmpeg {arguments}");
            var startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = startInfo })
            {
                var outputBuilder = new StringBuilder();
                var errorBuilder = new StringBuilder();

                //process.OutputDataReceived += (sender, e) =>
                //{
                //    if (e.Data != null)
                //    {
                //        outputBuilder.AppendLine(e.Data);
                //        Console.WriteLine($"[{config.Resolution}] {e.Data}");
                //    }
                //};
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        errorBuilder.AppendLine(e.Data);
                        Console.WriteLine($"==========> [{config.Resolution}] ERROR: {e.Data}");
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new Exception($"===========> FFmpeg process exited with code {process.ExitCode}. Error output:\n{errorBuilder}");
                }

                Console.WriteLine($"=========> Finished processing {config.Resolution}");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"=============> Error processing {config.Resolution}: {ex.Message}");
            //Console.WriteLine(ex.StackTrace);
        }
    }
    private static string CreateMasterPlaylist(string outputBaseFolder, List<BitrateConfig> configs)
    {

        var masterContent = "#EXTM3U\n#EXT-X-VERSION:3\n";
        foreach (var config in configs)
        {
            masterContent += $"#EXT-X-STREAM-INF:BANDWIDTH={(int)(float.Parse(config.Bitrate.TrimEnd('k')) * 1000)},RESOLUTION={config.Resolution}\n";
            masterContent += $"{config.OutputFolder}/{config.OutputFile}\n";
        }
        File.WriteAllText(Path.Combine(outputBaseFolder, "master.m3u8"), masterContent);
        return Path.Combine(outputBaseFolder, "master.m3u8");
    }
}