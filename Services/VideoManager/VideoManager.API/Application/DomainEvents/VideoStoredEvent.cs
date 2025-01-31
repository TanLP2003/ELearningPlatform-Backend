using Domain.DomainEvent;
using VideoManager.Domain.Entities;

namespace VideoManager.API.Application.DomainEvents;

public record VideoStoredSuccessedEvent(Video Video, Guid LectureId) : IDomainEvent;

public record VideoStoredFailedEvent(string DownloadedFolder) : IDomainEvent;