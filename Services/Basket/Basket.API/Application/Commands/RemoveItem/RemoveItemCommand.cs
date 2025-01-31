using Application.Messaging;
using Basket.API.Models;

namespace Basket.API.Application.Commands.RemoveItem;

public sealed record RemoveItemCommand(Guid UserId, Guid ItemId) : ICommand<Cart>;