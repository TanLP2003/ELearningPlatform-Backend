using Application.Messaging;
using Domain;
using MediatR;
using VideoManager.API.Application.DomainEvents;
using VideoManager.Domain.Contracts;
using VideoManager.Domain.Entities;

namespace VideoManager.API.Application.Commands.BeginProcessVideo;

public class BeginProcessVideoCommandHandler(ILocalStorageRepository repo, IPublisher publisher) : ICommandHandler<BeginProcessVideoCommand, Result>
{
    public async Task<Result> Handle(BeginProcessVideoCommand request, CancellationToken cancellationToken)
    {
        var userUploadFolder = repo.CreateUserUploadFolderIfNotExist(request.StorageFolder, request.UserId);
        var lectureFolder = repo.CreateLectureFolderIfNotExist(userUploadFolder, request.LectureId);
        var video = Video.Create(request.UserId, request.file.FileName);
        var videoFolder = repo.CreateVideosFolderForLecture(lectureFolder);
        await publisher.Publish(new VideoProcessStartedEvent(video, videoFolder, request.file, request.LectureId), cancellationToken);
        return Result.Success();
    }
}