using Application.Messaging;
using Basket.API.Models;
using Basket.API.Repository;

namespace Basket.API.Application.Queries.GetBasket;

public class GetBasketQueryHandler(ICartRepository repo) : IQueryHandler<GetBasketQuery, Cart>
{
    public async Task<Cart> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        var result = await repo.GetBasket(userId);
        if (result == null)
        {
            throw new Exception("Basket is not existed!!!");
        }
        return result;
    }
}