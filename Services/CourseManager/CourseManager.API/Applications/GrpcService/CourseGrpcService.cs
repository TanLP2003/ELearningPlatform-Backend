using CourseManager.API.Protos;
using CourseManager.Domain.Contracts;
using Grpc.Core;

namespace CourseManager.API.Applications.GrpcService;

public class CourseGrpcService(
    ILogger<CourseGrpcService> logger,
    ICourseRepository repo
    ) : CourseManagerProtoService.CourseManagerProtoServiceBase
{
    public override async Task<CourseBasicInfo> GetBasicCourseInfo(GetBasicCourseInfoRequest request, ServerCallContext context)
    {
        logger.LogInformation($"Receive gRPC request for courseId: {request.CourseId}");
        var courseId = Guid.Parse(request.CourseId);
        var result = await repo.GetById(courseId);
        var courseInfo = new CourseBasicInfo
        {
            CourseId = courseId.ToString(),
            CourseName = result.Title,
            AuthorId = result.InstructorId.ToString(),
            AuthorName = result.InstructorName,
            CourseImage = result.CourseImage ?? "default",
            Price = result.Price.ToString()
        };
        return courseInfo;
    }

    public override async Task<MultipleCourseBasicInfoResponse> GetMultipleBasicCourseInfo(MultipleBasicCourseInfoRequest request, ServerCallContext context)
    {
        logger.LogInformation("=========> Receive gRPC request for get multiple basic course info");
        var result = new MultipleCourseBasicInfoResponse();
        foreach (var req in request.Requests)
        {
            var courseBasicInfo = await GetBasicCourseInfo(req, context);
            result.Courses.Add(courseBasicInfo);
        }
        return result;
    }
}