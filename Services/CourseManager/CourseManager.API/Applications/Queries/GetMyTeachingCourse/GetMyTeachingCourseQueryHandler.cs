using Application.Messaging;
using CourseManager.Domain.Contracts;
using CourseManager.Domain.Entities;

namespace CourseManager.API.Applications.Queries.GetMyTeachingCourse;

public class GetMyTeachingCourseQueryHandler(
    ICourseRepository repo
    ) : IQueryHandler<GetMyTeachingCourseQuery, List<Course>>
{
    public async Task<List<Course>> Handle(GetMyTeachingCourseQuery request, CancellationToken cancellationToken)
    {
        return await repo.GetMyTeachingCourse(request.UserId);
    }
}