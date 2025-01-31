using Application.Messaging;
using CourseManager.Domain.Entities;
using Domain;

namespace CourseManager.API.Applications.Commands.MakeCoursePublic;

public record MakeCoursePublicCommand(Guid CourseId) : ICommand<Result<Course>>;