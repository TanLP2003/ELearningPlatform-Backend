using Application.Messaging;
using CourseManager.Domain.Entities;
using Domain;

namespace CourseManager.API.Applications.Queries.GetCourse;

public sealed record GetCourseQuery(Guid courseId) : IQuery<Course>;
