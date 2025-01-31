using Application.Messaging;
using CourseManager.Domain.Contracts;
using CourseManager.Domain.Entities;
using Domain;

namespace CourseManager.API.Applications.Commands.UpdateInstructorInfo;

public class UpdateInstructorInfoCommandHandler(ICourseRepository repo) : ICommandHandler<UpdateInstructorInfoCommand, Result>
{
    public async Task<Result> Handle(UpdateInstructorInfoCommand request, CancellationToken cancellationToken)
    {
        var list = await repo.GetMyTeachingCourse(request.TeacherId);
        foreach ( var course in list )
        {
            course.InstructorName = request.TeacherName;
        }
        await repo.SaveChangeAsync();
        return Result.Success();
    }
}