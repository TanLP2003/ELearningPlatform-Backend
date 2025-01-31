using Application.Messaging;
using CourseManager.Domain.Contracts;
using CourseManager.Domain.Entities;
using Domain;
using EventBus.Abstractions;
using EventBus.Events;
using System.Reflection.Metadata.Ecma335;

namespace CourseManager.API.Applications.Commands.MakeCoursePublic;

public class MakeCoursePublicCommandHandler(ICourseRepository repo, IEventBus eventBus) : ICommandHandler<MakeCoursePublicCommand, Result<Course>>
{
    public async Task<Result<Course>> Handle(MakeCoursePublicCommand request, CancellationToken cancellationToken)
    {
        var course = await repo.GetById(request.CourseId);
        if (course == null) return Result.Failure<Course>(Error.Create("Course.NullValue", "Course is not existed"));
        var result = course.MakeCoursePublic();
        if(result.IsFailure)
        {
            return Result.Failure<Course>(result.Error);
        }
        var @event = new CoursePublishedEvent
        {
            CourseId = course.Id,
            CourseTitle = course.Title,
            CourseImage = course.CourseImage,
            InstructorName = course.InstructorName
        };
        await eventBus.PublishEventAsync(@event);
        await repo.SaveChangeAsync();
        return course;
    }
}