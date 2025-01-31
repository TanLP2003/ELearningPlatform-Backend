using EventBus.Abstractions;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus;

public class EventBus(IPublishEndpoint publishEndpoint) : IEventBus
{
    public async Task PublishEventAsync(IntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        await publishEndpoint.Publish((dynamic)@event, cancellationToken);
    }
}