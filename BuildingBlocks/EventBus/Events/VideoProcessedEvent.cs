using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Events;

public record VideoProcessedEvent : IntegrationEvent
{
    public Guid VideoId { get; set; }
    public Guid LectureId { get; set; }
    public Guid UserId { get; set; }
    public string VideoName { get; set; } = default!;
    public string RawVideoPath { get; set; } = default!;
    public string ProcessedVideoPath { get; set; } = default!;
    public string VideoStatus { get; set; } = default!;
}