using Application.Messaging;
using Basket.API.Models;

namespace Basket.API.Application.Commands.EmptyBasket;

public record CleanBasketCommand(Guid UserId) : ICommand<Cart>;