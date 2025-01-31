using Application.Messaging;
using CourseManager.Domain.Contracts;
using CourseManager.Domain.Entities;
using Domain;

namespace CourseManager.API.Applications.Commands.AddDescriptionForLecture;

public class AddDescriptionForLectureCommandHandler(
    ICourseRepository repo,
    ILogger<AddDescriptionForLectureCommandHandler> logger
    ) : ICommandHandler<AddDescriptionForLectureCommand, Result<Course>>
{
    public async Task<Result<Course>> Handle(AddDescriptionForLectureCommand request, CancellationToken cancellationToken)
    {
        var lecture = await repo.GetLectureById(request.LectureId);
        if(lecture == null)
        {
            return Result.Failure<Course>(Error.Create("Lecture.NULL", $"Lecture with id: {request.LectureId} is not existed"));
        }
        lecture.Description = request.Description;
        var isSuccess = await repo.SaveChangeAsync();
        logger.LogInformation($"Result of add description: {isSuccess}");
        var course = await repo.GetById(request.CourseId);
        return course;
    }
}