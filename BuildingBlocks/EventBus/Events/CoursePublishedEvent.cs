using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Events;

public record CoursePublishedEvent : IntegrationEvent
{
    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; }
    public string? CourseImage { get; set; }
    public string InstructorName { get; set; }
}