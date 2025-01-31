using Application.Messaging;
using CourseManager.Domain.Entities;
using Domain;

namespace CourseManager.API.Applications.Commands.UpdateCourseInfo;

public record UpdateCourseInfoCommand : ICommand<Result<Course>>
{
    public Guid CourseId { get; set; }
    public string? Description { get; set; }
    public string? Title { get; set; }
    public string? Level { get; set; }
    public int? Price { get; set; }
    public Guid? CategoryId { get; set; }
    public string? Language { get; set; }
}