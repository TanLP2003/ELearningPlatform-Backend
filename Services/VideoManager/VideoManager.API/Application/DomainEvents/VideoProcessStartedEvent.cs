using Domain.DomainEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoManager.Domain.Entities;

namespace VideoManager.API.Application.DomainEvents;

public record VideoProcessStartedEvent(Video Video, string DownloadFolder, IFormFile File, Guid LectureId) : IDomainEvent;