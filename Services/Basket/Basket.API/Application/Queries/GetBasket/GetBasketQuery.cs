using Application.Messaging;
using Basket.API.Models;

namespace Basket.API.Application.Queries.GetBasket;

public record GetBasketQuery(Guid UserId) : IQuery<Cart>;