using Application.Messaging;
using CourseManager.Domain.Entities;

namespace CourseManager.API.Applications.Commands.AddVideoToLectureFromLibrary;

public record AddVideoToLectureFromLibraryCommand(Guid TeacherId, Guid CourseId, Guid LectureId, Guid VideoId) : ICommand<Course>;