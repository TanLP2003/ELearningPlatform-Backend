using Domain.DomainEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoManager.Domain.Entities;

namespace VideoManager.API.Application.DomainEvents;
public record VideoConvertSuccessedEvent(Video Video, Guid LectureId) : IDomainEvent;
public record VideoConvertFailedEvent(string DownloadedFolder) : IDomainEvent;