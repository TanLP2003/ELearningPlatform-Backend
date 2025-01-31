using Grpc.Core;
using LearningService.API.Infrastructure.Repositories;
using LearningService.API.Protos;

namespace LearningService.API.Applications.GrpcService
{
    public class LearningServiceGrpc(ILearningRepo repo) : LearningServiceProtoGrpc.LearningServiceProtoGrpcBase
    {
        public override async Task<AuthenticateRequestAccessResult> AuthenticateRequestAccessCourse(RequestAccessCourse request, ServerCallContext context)
        {
            var userId = Guid.Parse(request.UserId);
            var courseId = Guid.Parse(request.CourseId);
            var enrolledCourse = await repo.GetEnrolledCourseByUserIdAndCourseId(userId, courseId);
            if(enrolledCourse is null)
            {
                return new AuthenticateRequestAccessResult { Result = false };
            }
            return new AuthenticateRequestAccessResult { Result = true };
        }

        public override async Task<TotalCourseReviewResult> GetTotalCourseReviewData(RequestGetTotalCourseReview request, ServerCallContext context)
        {
            var courseId = Guid.Parse(request.CourseId);
            var (totalReviews, averageRating) = await repo.GetTotalReviewForCourse(courseId);
            return new TotalCourseReviewResult
            {
                ReviewCount = totalReviews,
                Average = averageRating
            };
        }
    }
}
