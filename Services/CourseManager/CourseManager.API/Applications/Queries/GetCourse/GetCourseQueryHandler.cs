using Application.Messaging;
using CourseManager.Domain.Contracts;
using CourseManager.Domain.Entities;

namespace CourseManager.API.Applications.Queries.GetCourse;


public class GetCourseQueryHandler(ICourseRepository repo) : IQueryHandler<GetCourseQuery, Course>
{
    public async Task<Course> Handle(GetCourseQuery request, CancellationToken cancellationToken)
    {
        var course = await repo.GetById(request.courseId) ?? throw new Exception("Course is not existed");
        return course;
    }
}