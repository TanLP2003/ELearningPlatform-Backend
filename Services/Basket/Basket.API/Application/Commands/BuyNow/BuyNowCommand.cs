using Application.Messaging;
using Basket.API.DTOs;
using Domain;

namespace Basket.API.Application.Commands.BuyNow;

public record BuyNowCommand : ICommand<string>
{
    public Guid UserId { get; set; } = default!;
    //public string UserName { get; set; } = default!;
    public Guid CourseId { get; set; } = default!;
    //public string CardName { get; set; } = default!;
    //public string CardNumber { get; set; } = default!;
    //public string Expiration { get; set; } = default!;
    //public string CVV { get; set; } = default!;
};