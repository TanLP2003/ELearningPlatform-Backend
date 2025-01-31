using Application.Messaging;
using CourseManager.Domain.Entities;
using CourseManager.Domain.Enums;

namespace CourseManager.API.Applications.Commands.CreateCourse;

public sealed record CreateCourseCommand : ICommand<Course>
{
    public string Title { get; set; } = default!;
    public CourseLevel Level { get; set; }
    public Guid InstructorId { get; set; }
    public string InstructorName { get; set; }
    public Guid CategoryId { get; set; } = default!;
}