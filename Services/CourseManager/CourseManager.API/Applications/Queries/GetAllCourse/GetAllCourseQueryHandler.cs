using Application.Messaging;
using CourseManager.Domain.Contracts;
using CourseManager.Domain.Entities;
using Domain;

namespace CourseManager.API.Applications.Queries.GetAllCourse
{
    public class GetAllCourseQueryHandler(ICourseRepository repo) : IQueryHandler<GetAllCourseQuery, Result<List<Course>>>
    {
        public async Task<Result<List<Course>>> Handle(GetAllCourseQuery request, CancellationToken cancellationToken)
        {
            var courses = await repo.GetAllAsync();
            return courses;
        }
    }
}
