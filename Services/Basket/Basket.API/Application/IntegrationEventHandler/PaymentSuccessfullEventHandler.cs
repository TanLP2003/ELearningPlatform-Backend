using Basket.API.Models;
using Basket.API.Repository;
using EventBus.Abstractions;
using EventBus.Events;
using EventBus.Events.Payment;
using MassTransit;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace Basket.API.Application.IntegrationEventHandler
{
    public class PaymentSuccessfullEventHandler(ILogger<PaymentSuccessfullEventHandler> logger, ICartRepository repo, IEventBus eventBus) : IIntegrationEventHandler<PaymentSuccessfulEvent>
    {
        public async Task Consume(ConsumeContext<PaymentSuccessfulEvent> context)
        {
            logger.LogInformation($"========> Receive PaymentEvent ${context.Message}");
            var message = context.Message;
            var outboxMessage = await repo.GetOutboxMessageByEventId(message.RelatedPayEventId);
            logger.LogInformation($"========> EventId: {message.RelatedPayEventId}");
            logger.LogInformation($"========> Outbox Message: {outboxMessage}");
            IntegrationEvent? @event = JsonConvert
                .DeserializeObject<IntegrationEvent>(
                    outboxMessage.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                        Formatting = Formatting.Indented,
                    }
                );
            OrderCreatedEvent orderCreatedEvent;
            logger.LogInformation($"=========> Event type is {@event.GetType()}");
            if (@event is BasketCheckoutedEvent)
            {
                BasketCheckoutedEvent basketCheckoutedEvent = (BasketCheckoutedEvent)@event;
                orderCreatedEvent = MapBasketCheckoutEventToOrderCreatedEvent(basketCheckoutedEvent);
                await repo.CleanBasket(basketCheckoutedEvent.CustomerId);
                await repo.CommitAsync();
            }
            else
            {
                BuyNowDoneEvent buyNowDoneEvent = (BuyNowDoneEvent)@event;
                orderCreatedEvent = MapBuyNowDoneEventToOrderCreatedEvent(buyNowDoneEvent);
            }

            await eventBus.PublishEventAsync(orderCreatedEvent);
        }

        private OrderCreatedEvent MapBasketCheckoutEventToOrderCreatedEvent(BasketCheckoutedEvent @event)
        {
            List<OrderItemCreatedEvent> orderItems = [];
            foreach (var item in @event.Items)
            {
                orderItems.Add(new OrderItemCreatedEvent
                {
                    CourseId = item.CourseId,
                    CourseName = item.CourseName,
                    CourseImage = item.CourseImage,
                    AuthorId = item.AuthorId,
                    AuthorName = item.AuthorName,
                    Price = item.Price
                });
            }

            var newOrderCreatedEvent = new OrderCreatedEvent
            {
                UserId = @event.CustomerId,
                Items = orderItems
            };
            return newOrderCreatedEvent;
        }

        private OrderCreatedEvent MapBuyNowDoneEventToOrderCreatedEvent(BuyNowDoneEvent @event)
        {
            List<OrderItemCreatedEvent> orderItems = [];

            orderItems.Add(new OrderItemCreatedEvent
            {
                CourseId = @event.Item.CourseId,
                CourseName = @event.Item.CourseName,
                CourseImage = @event.Item.CourseImage,
                AuthorId = @event.Item.AuthorId,
                AuthorName = @event.Item.AuthorName,
                Price = @event.Item.Price
            });

            var newOrderCreatedEvent = new OrderCreatedEvent
            {
                UserId = @event.CustomerId,
                Items = orderItems
            };
            return newOrderCreatedEvent;
        }
    }
}
