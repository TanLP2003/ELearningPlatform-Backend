using EventBus.Abstractions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EventBus;

public static class EventBusExtension
{
    public static void AddMessageBroker(this IServiceCollection service, IConfiguration configuration, Assembly? assembly = null)
    {
        service.AddMassTransit(busConfig =>
        {
            busConfig.SetKebabCaseEndpointNameFormatter();
            if (assembly != null) busConfig.AddConsumers(assembly);
            busConfig.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(configuration["MessageBroker:Host"]!), h =>
                {
                    h.Username(configuration["MessageBroker:Username"]!);
                    h.Password(configuration["MessageBroker:Password"]!);
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        service.AddScoped<IEventBus, EventBus>();
    }
}