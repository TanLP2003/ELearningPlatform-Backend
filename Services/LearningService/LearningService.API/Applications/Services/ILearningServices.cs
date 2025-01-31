using Domain;
using LearningService.API.Entities;

namespace LearningService.API.Applications.Services
{
    public interface ILearningServices
    {
        Task<List<EnrolledCourse>> GetAllEnrolledCoursesByUserId(Guid userId);
        Task<Result<EnrolledCourse>> GetEnrolledCourseByUserIdAndCourseId(Guid userId, Guid courseId);
        Task<Result<EnrolledCourse>> ArchiveOrUnarchiveEnrolledCourse(Guid userId, Guid courseId, bool setArchived);

        Task<Result<LearningNote>> AddLearningNote(Guid userId, Guid courseId, Guid lectureId, string content, TimeSpan noteAt);
        Task<Result<LearningNote>> UpdateLearningNote(Guid userId, Guid courseId, Guid lectureId, TimeSpan timeSpan, string content);
        Task<Result> DeleteLearningNote(Guid userId, Guid courseId, Guid lectureId, TimeSpan timeSpan);
        Task<List<LearningNote>> GetUserLearningNotesForEnrolledCourse(Guid userId, Guid courseId);

        Task<Result<EnrolledCourse>> AddCourseReview(Guid userId, Guid courseId, int rating, string? reviewText);
        Task<Result<EnrolledCourse>> UpdateCourseReview(Guid userId, Guid courseId, int rating, string? reviewText);
        Task<Result> DeleteCourseReview(Guid userId, Guid courseId);
    }
}
