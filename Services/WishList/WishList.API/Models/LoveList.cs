namespace WishList.API.Models;

public class LoveList
{
    public Guid UserId { get; set; }
    public List<LoveListItem> Items { get; set; } = [];
}