using EventBus.Abstractions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;

namespace Basket.API.Infrastructure;

public class ProcessOutBoxMessageJob(CartDbContext dbContext, IEventBus bus, ILogger<ProcessOutBoxMessageJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Executing job!!!");
        var messages = await dbContext.OutboxMessages
            .Where(m => m.ProcessedOn == null)
            .Take(10)
            .ToListAsync(context.CancellationToken);
        foreach(var outboxMessage in messages)
        {
            IntegrationEvent? @event = JsonConvert
                .DeserializeObject<IntegrationEvent>(
                    outboxMessage.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                        Formatting = Formatting.Indented,
                    }
                );
            if (@event == null) continue;
            logger.LogInformation($"----------------> Type of event: {@event.GetType()}");
            //await publishEndpoint.Publish((dynamic)@event);
            await bus.PublishEventAsync(@event);
            logger.LogInformation($"Publish event: {JsonConvert.SerializeObject(@event, Formatting.Indented)}");
            outboxMessage.ProcessedOn = DateTime.UtcNow;
        }
        await dbContext.SaveChangesAsync();
    }
} 