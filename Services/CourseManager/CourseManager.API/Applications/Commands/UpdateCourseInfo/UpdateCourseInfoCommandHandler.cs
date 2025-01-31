using Application.Messaging;
using CourseManager.Domain.Contracts;
using CourseManager.Domain.Entities;
using Domain;

namespace CourseManager.API.Applications.Commands.UpdateCourseInfo;

public class UpdateCourseInfoCommandHandler(ICourseRepository repo) : ICommandHandler<UpdateCourseInfoCommand, Result<Course>>
{
    public async Task<Result<Course>> Handle(UpdateCourseInfoCommand request, CancellationToken cancellationToken)
    {
        var course = await repo.GetById(request.CourseId);
        if(course is null)
        {
            return Result.Failure<Course>(Error.Create("Course.NullValue", $"Course {request.CourseId} is not existed"));
        }
        Console.WriteLine($"{request.Price}");
        var result = course.UpdateCourseInfo(request.Title, request.Description, request.Level, request.Price, request.CategoryId, request.Language);
        if(result.IsFailure)
        {
            return Result.Failure<Course>(result.Error);
        }
        await repo.SaveChangeAsync();
        return course;
    }
}