using Application.Messaging;
using CourseManager.Domain.Entities;
using Domain;

namespace CourseManager.API.Applications.Queries.GetAllCourse;

public sealed record GetAllCourseQuery : IQuery<Result<List<Course>>>;