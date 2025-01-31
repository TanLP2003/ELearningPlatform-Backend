using Application.Messaging;
using Basket.API.Models;

namespace Basket.API.Application.Commands.AddCartItem;

public sealed record AddCartItemCommand(Guid UserId, CartItem Item) : ICommand<Cart>;