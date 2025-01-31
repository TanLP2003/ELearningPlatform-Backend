using Application.Messaging;
using CourseManager.Domain.Entities;
using Domain;

namespace CourseManager.API.Applications.Commands.AddLectureToSection;
public record AddLectureToSectionCommand(Guid CourseId, Guid SectionId, string Title) : ICommand<Result<Course>>;