using Application.Messaging;
using VideoManager.Domain.Entities;
using VideoManager.Domain.ValueObject;

namespace VideoManager.API.Application.Commands.DownloadVideo;

public record DownloadVideoCommand(Video Video, string DownloadFolder, IFormFile File, Guid LectureId) : ICommand;