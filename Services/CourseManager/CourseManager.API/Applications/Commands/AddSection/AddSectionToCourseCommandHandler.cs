using Application.Messaging;
using CourseManager.Domain.Contracts;
using CourseManager.Domain.Entities;
using Domain;

namespace CourseManager.API.Applications.Commands.AddSection;

public class AddSectionToCourseCommandHandler(ICourseRepository repo) : ICommandHandler<AddSectionToCourseCommand, Result<Course>>
{
    public async Task<Result<Course>> Handle(AddSectionToCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await repo.GetById(request.CourseId) ?? throw new Exception("Course is not existed");
        var result = course.AddSection(request.SectionName);
        if(result.IsSuccess)
        {
            await repo.CreateSection(result.Value);
            await repo.SaveChangeAsync();
        }
        else
        {
            return Result.Failure<Course>(result.Error);
        }
        return course;
    }
}