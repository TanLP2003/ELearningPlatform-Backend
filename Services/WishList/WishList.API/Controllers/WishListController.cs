using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WishList.API.Application.Services;

namespace WishList.API.Controllers
{
    [Route("api/wishlist")]
    [ApiController]
    public class WishListController(
          ILoveListService services
        ) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetWishList()
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId == null) return Unauthorized();
            var result = await services.GetWishList(Guid.Parse(userId));
            return Ok(result);
        }

        [HttpPut("add-to-wish-list/{courseId}")]
        public async Task<IActionResult> AddToWishList(Guid courseId)
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId == null) return Unauthorized();
            var result = await services.LikeCourse(Guid.Parse(userId), courseId);
            return Ok(result);
        }

        [HttpPut("remove-from-wish-list/{courseId}")]
        public async Task<IActionResult> RemoveFromWishList(Guid courseId)
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId == null) return Unauthorized();
            var result = await services.UnlikeCoures(Guid.Parse(userId), courseId);
            return Ok(result);
        }
    }
}
