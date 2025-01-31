using Application.Messaging;
using AutoMapper;
using Basket.API.Application.Commands.CheckoutBasket;
using Basket.API.Models;
using Basket.API.Protos;
using Basket.API.Repository;
using Domain;
using EventBus.Events;
using Newtonsoft.Json;

namespace Basket.API.Application.Commands.BuyNow;

public class BuyNowCommandHandler(
    ILogger<BuyNowCommandHandler> logger, ICartRepository repo, IMapper mapper,
    CourseManagerProtoService.CourseManagerProtoServiceClient client,
    PaymentServiceProtoGrpc.PaymentServiceProtoGrpcClient paymentServiceClient
) : ICommandHandler<BuyNowCommand, string>
{
    public async Task<string> Handle(BuyNowCommand request, CancellationToken cancellationToken)
    {
        var courseInfo = await client.GetBasicCourseInfoAsync(new GetBasicCourseInfoRequest
        {
            CourseId = request.CourseId.ToString()
        });

        var buyNowDoneEvent = new BuyNowDoneEvent
        {
            CustomerId = request.UserId,
            //CustomerName = request.UserName,
            Item = new BasketItemCheckoutedEvent
            {
                CourseId = Guid.Parse(courseInfo.CourseId),
                CourseName = courseInfo.CourseName,
                CourseImage= courseInfo.CourseImage,
                AuthorId = Guid.Parse(courseInfo.AuthorId),
                AuthorName = courseInfo.AuthorName,
                Price = decimal.Parse(courseInfo.Price)
            },
            TotalPrice = int.Parse(courseInfo.Price),
            //CardName = request.CardName,
            //CardNumber = request.CardNumber,
            //CVV = request.CVV,
            //Expiration = request.Expiration,
        };
        var outBoxMessage = new OutboxMessage(
            buyNowDoneEvent.GetType().Name,
            JsonConvert.SerializeObject(
                buyNowDoneEvent,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    Formatting = Formatting.Indented
                })
        );
        await repo.AddOutboxMessage(outBoxMessage);
        await repo.CommitAsync();
        var paymentData = await paymentServiceClient.GeneratePayUrlAsync(new PayRequest
        {
            PayEventId = outBoxMessage.EventId.ToString(),
            Amount = (int)buyNowDoneEvent.TotalPrice
        });
        return paymentData.PayUrl;
    }
}