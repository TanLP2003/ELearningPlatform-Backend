using Application.Messaging;
using Basket.API.Models;
using Basket.API.Repository;

namespace Basket.API.Application.Commands.EmptyBasket;

public class CleanBasketCommandHandler(ICartRepository repo) : ICommandHandler<CleanBasketCommand, Cart>
{
    public async Task<Cart> Handle(CleanBasketCommand request, CancellationToken cancellationToken)
    {
        var result = await repo.CleanBasket(request.UserId);
        await repo.CommitAsync();
        return result;
    }
}