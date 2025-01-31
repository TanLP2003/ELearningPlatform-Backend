using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoLibrary.API.Application.Services;

namespace VideoLibrary.API.Controllers
{
    [Route("api/video-library")]
    [ApiController]
    public class VideoLibraryController(IVideoLibraryService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUploadedListByUser()
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId == null)
            {
                return Unauthorized();
            }
            var result = await service.GetFileCollectionByUser(Guid.Parse(userId));
            return Ok(result);
        }
    }
}
