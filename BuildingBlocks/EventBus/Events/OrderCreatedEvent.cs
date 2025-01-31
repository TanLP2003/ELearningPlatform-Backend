using EventBus.Abstractions;

namespace EventBus.Events;

public record OrderCreatedEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public List<OrderItemCreatedEvent> Items { get; set; } = [];
}