using Application.Messaging;
using CourseManager.Domain.Contracts;
using CourseManager.Domain.Entities;
using MediatR;

namespace CourseManager.API.Applications.Commands.CreateCourse;

public sealed class CreateCourseCommandHandler(ICourseRepository repo) : ICommandHandler<CreateCourseCommand, Course>
{
    public async Task<Course> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var newCourse = Course.Create(request.Title, request.Level, request.InstructorId, request.CategoryId, request.InstructorName);
        await repo.CreateCourse(newCourse);
        await repo.SaveChangeAsync();  
        return newCourse;
    }   
}