using Domain;
using MongoDB.Bson;

namespace Basket.API.Models;

public class Cart
{
    public Guid UserId { get; set; }
    public List<CartItem> Items { get; set; } = [];
    public decimal TotalPrice => Items.Sum(x => x.Price);

    public Cart(Guid userId)
    {
        UserId = userId;
    }
}