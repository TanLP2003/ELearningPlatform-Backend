using Application.Messaging;
using CourseManager.Domain.Entities;

namespace CourseManager.API.Applications.Queries.GetMyTeachingCourse;

public record GetMyTeachingCourseQuery(Guid UserId) : IQuery<List<Course>>;