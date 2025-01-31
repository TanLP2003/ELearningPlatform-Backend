using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Events;

public record BasketCheckoutedEvent : IntegrationEvent
{
    public Guid CustomerId { get; set; } = default!;
    //public string CustomerName { get; set; } = default!;
    public List<BasketItemCheckoutedEvent> Items { get; set; } = [];
    public decimal TotalPrice { get; set; } = default!;
    //public string CardName { get; set; } = default!;
    //public string CardNumber { get; set; } = default!;
    //public string Expiration { get; set; } = default!;
    //public string CVV { get; set; } = default!;
};