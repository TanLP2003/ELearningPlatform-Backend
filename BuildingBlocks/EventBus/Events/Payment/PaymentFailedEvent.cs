﻿using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Events.Payment
{
    public record PaymentFailedEvent : IntegrationEvent
    {
    }
}
