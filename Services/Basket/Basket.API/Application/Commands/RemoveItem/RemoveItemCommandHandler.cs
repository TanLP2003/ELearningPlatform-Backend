using Application.Messaging;
using Basket.API.Models;
using Basket.API.Repository;
using MediatR;

namespace Basket.API.Application.Commands.RemoveItem;

public class RemoveItemCommandHandler(ICartRepository repo) : ICommandHandler<RemoveItemCommand, Cart>
{
    public async Task<Cart> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await repo.GetBasket(request.UserId);
        var item = cart.Items.FirstOrDefault(item => item.CourseId == request.ItemId);
        if (item is not null)
        {
            cart = await repo.RemoveCartItem(request.UserId, item);
            await repo.CommitAsync();
        }
        return cart;
    }
}