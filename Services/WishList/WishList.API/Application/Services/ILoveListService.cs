using WishList.API.Models;

namespace WishList.API.Application.Services;

public interface ILoveListService
{
    Task<LoveList> GetWishList(Guid userId);
    Task<LoveList> LikeCourse(Guid userId, Guid courseId);
    Task<LoveList> UnlikeCoures(Guid userId, Guid couresId);
}