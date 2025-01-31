using Application.Messaging;
using AutoMapper;
using Basket.API.Models;
using Basket.API.Protos;
using Basket.API.Repository;
using EventBus.Abstractions;
using EventBus.Events;
using MediatR;
using Newtonsoft.Json;

namespace Basket.API.Application.Commands.CheckoutBasket;

public class CheckoutBasketCommandHandler(
    ILogger<CheckoutBasketCommandHandler> logger, 
    ICartRepository repo, 
    IMapper mapper,
    PaymentServiceProtoGrpc.PaymentServiceProtoGrpcClient paymentServiceClient) : ICommandHandler<CheckoutBasketCommand, string>
{
    public async Task<string> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
    {
        var userBasket = await repo.GetBasket(request.UserId);
        var checkoutBasketEvent = new BasketCheckoutedEvent
        {
            CustomerId = request.UserId,
            //CustomerName = request.UserName,
            Items = mapper.Map<List<BasketItemCheckoutedEvent>>(userBasket.Items),
            TotalPrice = userBasket.TotalPrice,
            //CardName = request.CardName,
            //CardNumber = request.CardNumber,
            //CVV = request.CVV,
            //Expiration = request.Expiration,
        };
        //logger.LogInformation($"Number items of collection: {checkoutBasketEvent.Items.Count}");
        var outBoxMessage = new OutboxMessage(
                checkoutBasketEvent.GetType().Name,
                JsonConvert.SerializeObject(
                    checkoutBasketEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                        Formatting = Formatting.Indented
                    })
            );
        logger.LogInformation($"-----> Outbox message: {JsonConvert.SerializeObject(outBoxMessage, Formatting.Indented)}");

        //var result = await repo.CleanBasket(request.UserId);
        await repo.AddOutboxMessage(outBoxMessage);
        await repo.CommitAsync();
        //if (result != null)
        //{
        //    Console.WriteLine("Publish BasketCheckoutEvent");
        //}
        var paymentData = await paymentServiceClient.GeneratePayUrlAsync(new PayRequest
        {
            PayEventId = outBoxMessage.EventId.ToString(),
            Amount = (int)userBasket.TotalPrice
        });
        return paymentData.PayUrl;
    }
}