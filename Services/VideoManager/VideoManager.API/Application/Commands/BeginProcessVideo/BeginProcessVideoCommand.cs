using Application.Messaging;
using Domain;

namespace VideoManager.API.Application.Commands.BeginProcessVideo;

public record BeginProcessVideoCommand(string StorageFolder, Guid UserId, Guid LectureId, IFormFile file) : ICommand<Result>;