using WishList.API.Models;

namespace WishList.API.Repository;

public interface ILoveListRepository
{
    Task<LoveList> GetWishList(Guid userId);
    Task<int> CommitAsync();
}