using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Events;

public class OrderItemCreatedEvent
{
    public Guid CourseId { get; set; }
    public string? CourseName { get; set; }
    public string? CourseImage { get; set; }
    public Guid AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public decimal Price { get; set; }
}