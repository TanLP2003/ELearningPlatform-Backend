using Domain.DomainEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoManager.Domain.Entities;

namespace VideoManager.API.Application.DomainEvents;

public record VideoDownloadSuccessedEvent(Video Video, string DownloadedPath, string DownloadedFolder, Guid LectureId) : IDomainEvent;

public record VideoDownloadFailedEvent(string DownloadedFolder) : IDomainEvent;