using Application.Messaging;
using Basket.API.Models;
using Basket.API.Repository;

namespace Basket.API.Application.Commands.AddCartItem;

public class AddCartItemCommandHandler(ICartRepository repo) : ICommandHandler<AddCartItemCommand, Cart>
{
    public async Task<Cart> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await repo.GetBasket(request.UserId);
        var itemExisted = cart.Items.Any(i => i.CourseId == request.Item.CourseId);
        if (!itemExisted)
        {
            cart = await repo.AddCartItem(request.UserId, request.Item);
        }
        await repo.CommitAsync();
        return cart;
    }
}