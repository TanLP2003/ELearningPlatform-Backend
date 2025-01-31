using Application.Messaging;
using Basket.API.DTOs;
using Basket.API.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace Basket.API.Application.Commands.CheckoutBasket;

public sealed record CheckoutBasketCommand : ICommand<string>
{
    public Guid UserId { get; set; } = default!;
    //public string UserName { get; set; } = default!;
    //public string CardName { get; set; } = default!;
    //public string CardNumber { get; set; } = default!;
    //public string Expiration { get; set; } = default!;
    //public string CVV { get; set; } = default!;
}