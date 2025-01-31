using LearningService.API.Entities;

namespace LearningService.API.Infrastructure.Repositories
{
    public interface ILearningRepo
    {
        Task<List<EnrolledCourse>> GetAllEnrolledCoursesByUserId(Guid userId);
        Task<EnrolledCourse?> GetEnrolledCourseByUserIdAndCourseId(Guid userId, Guid courseId);
        Task<EnrolledCourse> AddEnrolledCourse(EnrolledCourse enrolledCourse);
        Task<List<EnrolledCourse>> AddManyEnrolledCourses(List<EnrolledCourse> enrolledCourses);

        Task<LearningNote> AddLearningNote(LearningNote learningNote);
        Task<int> DeleteLearningNote(LearningNote learningNote);
        Task<List<LearningNote>> GetUserLearningNotesForEnrolledCourse(Guid userId, Guid courseId);
        Task<LearningNote?> GetLearningNoteByTimeSpan(Guid userId, Guid courseId, Guid lectureId, TimeSpan duration);

        Task<CourseReview> AddCourseReview(CourseReview review);
        Task<int> DeleteCourseReview(CourseReview courseReview);

        Task<(int, double)> GetTotalReviewForCourse(Guid courseId);
        Task<int> SaveChangeAsync();
    }
}
