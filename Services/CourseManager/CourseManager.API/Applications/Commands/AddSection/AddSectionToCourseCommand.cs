using Application.Messaging;
using CourseManager.Domain.Entities;
using Domain;

namespace CourseManager.API.Applications.Commands.AddSection;

public sealed record AddSectionToCourseCommand(Guid CourseId, string SectionName) : ICommand<Result<Course>>;