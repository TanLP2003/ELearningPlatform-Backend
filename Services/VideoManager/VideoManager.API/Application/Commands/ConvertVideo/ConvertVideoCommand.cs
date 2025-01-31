using Application.Messaging;
using Domain.DomainEvent;
using VideoManager.Domain.Entities;

namespace VideoManager.API.Application.Commands.ProcessVideo;

public sealed record ConvertVideoCommand(Video Video, string VideoDownloadedPath, string DownloadedFolder, Guid LectureId) : ICommand;