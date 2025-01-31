using Application.Messaging;
using CourseManager.Domain.Entities;
using Domain;

namespace CourseManager.API.Applications.Commands.AddDescriptionForLecture;

public record AddDescriptionForLectureCommand : ICommand<Result<Course>>
{
    public Guid CourseId { get; set; }
    public string Description { get; set; }
    public Guid LectureId { get; set; }
}