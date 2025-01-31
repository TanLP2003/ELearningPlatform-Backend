using Application.Messaging;
using CourseManager.API.Protos;
using CourseManager.Domain.Contracts;
using CourseManager.Domain.Entities;

namespace CourseManager.API.Applications.Commands.AddVideoToLectureFromLibrary;

public class AddVideoToLectureFromLibraryCommandHandler(
    VideoManagerProtoService.VideoManagerProtoServiceClient client,
    ICourseRepository repo) : ICommandHandler<AddVideoToLectureFromLibraryCommand, Course>
{
    public async Task<Course> Handle(AddVideoToLectureFromLibraryCommand request, CancellationToken cancellationToken)
    {
        var videoInfo = await client.GetVideoInfoAsync(new GetVideoInfoRequest
        {
            UserId = request.TeacherId.ToString(),
            VideoId = request.VideoId.ToString()
        });
        if (videoInfo == null)
        {
            throw new ArgumentNullException("Video is null");
        }
        var lecture = await repo.GetLectureById(request.LectureId);
        lecture.LectureContentUrl = videoInfo.VideoProcessedPath;
        lecture.VideoName = videoInfo.VideoName;
        await repo.SaveChangeAsync();
        var course = await repo.GetById(request.CourseId);
        return course;
    }
}