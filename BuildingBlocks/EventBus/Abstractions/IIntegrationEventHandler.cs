﻿using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Abstractions;

public interface IIntegrationEventHandler<TEvent> : IConsumer<TEvent>
    where TEvent : IntegrationEvent
{

}