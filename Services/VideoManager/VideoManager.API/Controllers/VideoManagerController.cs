
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using VideoManager.API.Application.Commands.BeginProcessVideo;
using VideoManager.API.Application.Commands.DownloadVideo;
using VideoManager.API.Application.Services;

namespace VideoManager.API.Controllers
{
    [Route("api/video-manager")]
    [ApiController]
    public class VideoManagerController(ISender sender, IWebHostEnvironment env) : ControllerBase
    {
        [HttpGet]
        public IActionResult TestPath()
        {
            var contentRootPaht = env.ContentRootPath;
            var webRootPath = env.WebRootPath;
            var result = new ResponseDto
            {
                ContentRootPath = contentRootPaht,
                WebRootPath = webRootPath
            };
            return Ok(result);
        }
        [HttpGet("test-cli")]
        public async Task<IActionResult> TestCli()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = "-version",
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,   
            };
            var proc = Process.Start(startInfo);
            ArgumentNullException.ThrowIfNull(proc);
            string output = proc.StandardOutput.ReadToEnd();    
            await proc.WaitForExitAsync();
            return Ok(output);
        }
        [HttpPost("{lectureId}")]
        //[RequestSizeLimit(52428800)]
        public async Task<IActionResult> UploadVideo(IFormFile file, Guid lectureId)
        {
            var userId = Guid.Parse(HttpContext.Request.Headers["X-User-Id"].ToString());
            string currentDirectory = Directory.GetCurrentDirectory();
            var beginProcessVideoCommand = new BeginProcessVideoCommand(Path.Combine(currentDirectory, "storage"), userId, lectureId, file);
            var result = await sender.Send(beginProcessVideoCommand);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);  
        }
    }
    public class ResponseDto
    {
        public string? RootDir { get; set; }
        public string? ContentRootPath { get; set; }
        public string? WebRootPath { get; set; }
    }
}
