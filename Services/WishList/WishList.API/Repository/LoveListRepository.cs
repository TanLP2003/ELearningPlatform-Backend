using Microsoft.EntityFrameworkCore;
using WishList.API.Models;

namespace WishList.API.Repository;

public class LoveListRepository(WishListContext dbContext) : ILoveListRepository
{
    public async Task<int> CommitAsync()
    {
        return await dbContext.SaveChangesAsync();
    }

    public async Task<LoveList> GetWishList(Guid userId)
    {
        var wishList = await dbContext.LoveLists.FirstOrDefaultAsync(w => w.UserId == userId);
        if(wishList is null)
        {
            wishList = new LoveList { UserId = userId };
            await dbContext.AddAsync(wishList);
            await dbContext.SaveChangesAsync();
        }
        return wishList;
    }

}