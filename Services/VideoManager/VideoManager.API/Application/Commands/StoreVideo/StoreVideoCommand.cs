using Application.Messaging;
using VideoManager.Domain.Entities;

namespace VideoManager.API.Application.Commands.StoreVideo;

public record StoreVideoCommand(Video Video, Guid LectureId) : ICommand;