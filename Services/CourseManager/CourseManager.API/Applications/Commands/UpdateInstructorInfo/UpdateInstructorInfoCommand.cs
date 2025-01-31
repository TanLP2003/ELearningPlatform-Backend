using Application.Messaging;
using CourseManager.Domain.Entities;
using Domain;

namespace CourseManager.API.Applications.Commands.UpdateInstructorInfo;

public sealed record UpdateInstructorInfoCommand(Guid TeacherId, string TeacherName) : ICommand<Result>;
