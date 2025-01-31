using Application.Messaging;
using CourseManager.Domain.Contracts;
using CourseManager.Domain.Entities;
using Domain;
using MediatR;

namespace CourseManager.API.Applications.Commands.AddLectureToSection;

public class AddLectureToSectionCommandHandler(ICourseRepository repo) : ICommandHandler<AddLectureToSectionCommand, Result<Course>>
{
    public async Task<Result<Course>> Handle(AddLectureToSectionCommand request, CancellationToken cancellationToken)
    {
        var course = await repo.GetById(request.CourseId) ?? throw new Exception("Course is not existed");
        var result = course.AddLectureToSection(request.SectionId, request.Title);
        if(result.IsFailure)
        {
            return Result.Failure<Course>(Error.Create("Error.Lecture", "Failed to add lecture"));
        }
        await repo.AddLectureToSection(result.Value);
        await repo.SaveChangeAsync();
        return course;
    }
}