using Basket.API.Models;
using System.Threading.Tasks;

namespace Basket.API.Repository;

public interface ICartRepository
{
    Task<Cart> GetBasket(Guid userId);
    Task<Cart> AddCartItem(Guid userId, CartItem item);
    Task<Cart> RemoveCartItem(Guid userId, CartItem item);
    Task<Cart> CleanBasket(Guid userId);
    Task AddOutboxMessage(OutboxMessage outboxMessage);
    Task<OutboxMessage> GetOutboxMessageByEventId(Guid outboxEventId);
    Task CommitAsync();
    //Task<Cart> CheckoutBasket(Guid userId); 
}