using Grpc.Core;
using WishList.API.Models;
using WishList.API.Protos;
using WishList.API.Repository;

namespace WishList.API.Application.Services;

public class LoveListService(
    ILoveListRepository repo,
    ILogger<LoveListService> logger,
    CourseManagerProtoService.CourseManagerProtoServiceClient client
    ) : ILoveListService
{
    public async Task<LoveList> GetWishList(Guid userId)
    {
        return await repo.GetWishList(userId);
    }

    public async Task<LoveList> LikeCourse(Guid userId, Guid courseId)
    {
        try
        {
            var wishList = await repo.GetWishList(userId);
            var wishListItem = wishList.Items.FirstOrDefault(item => item.CourseId == courseId);
            if (wishListItem != null) return wishList;
            var courseInfo = await client.GetBasicCourseInfoAsync(new GetBasicCourseInfoRequest { CourseId = courseId.ToString() });
            wishListItem = new LoveListItem
            {
                CourseId = Guid.Parse(courseInfo.CourseId),
                CourseName = courseInfo.CourseName,
                CourseImage = courseInfo.CourseImage,
                AuthorId = Guid.Parse(courseInfo.AuthorId),
                AuthorName = courseInfo.AuthorName,
                Price = decimal.Parse(courseInfo.Price)
            };
            wishList.Items.Add(wishListItem);
            await repo.CommitAsync();
            return wishList;
        }
        catch(RpcException ex)
        {
            logger.LogError($"gRPC call failed: {ex.Status.Detail}");
            throw new ApplicationException("Failed to like course due to a communication error", ex);
        }
    }

    public async Task<LoveList> UnlikeCoures(Guid userId, Guid courseId)
    {
        var wishList = await repo.GetWishList(userId);
        var wishListItem = wishList.Items.FirstOrDefault(item => item.CourseId == courseId);
        if(wishListItem != null)
        {
            wishList.Items.Remove(wishListItem);
            await repo.CommitAsync();   
        }
        return wishList;
    }
}