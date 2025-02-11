﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Abstractions;

public interface IEventBus
{
    Task PublishEventAsync(IntegrationEvent @event, CancellationToken cancellationToken = default);
}