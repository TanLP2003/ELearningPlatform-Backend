using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Events
{
    public record CourseReviewedEvent : IntegrationEvent
    {
        public Guid CourseId { get; set; }
    }
}
