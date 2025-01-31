using Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Basket.API.Models;

public class OutboxMessage
{
    public OutboxMessage()
    {
    }
    public OutboxMessage(string eventType, string content)
    {
        EventId = Guid.NewGuid();
        EventType = eventType;
        Content = content;
        OccurOn = DateTime.UtcNow;
    }
    public Guid EventId { get; set; }
    public string EventType { get; set; }  = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime OccurOn { get; set; }
    public DateTime? ProcessedOn { get; set; }   
}