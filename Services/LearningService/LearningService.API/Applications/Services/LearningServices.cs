using Domain;
using LearningService.API.Entities;
using LearningService.API.Infrastructure.Repositories;

namespace LearningService.API.Applications.Services
{
    public class LearningServices(ILearningRepo repo) : ILearningServices
    {
        public async Task<List<EnrolledCourse>> GetAllEnrolledCoursesByUserId(Guid userId)
        {
            return await repo.GetAllEnrolledCoursesByUserId(userId);
        }

        public async Task<Result<EnrolledCourse>> GetEnrolledCourseByUserIdAndCourseId(Guid userId, Guid courseId)
        {
            var enrolledCourse = await repo.GetEnrolledCourseByUserIdAndCourseId(userId, courseId);
            if(enrolledCourse is null)
            {
                return Result.Failure<EnrolledCourse>(
                    Error.Create("EnrolledCourse.Null", $"User {userId} is not enrolled course {courseId}")
                );
            }
            return enrolledCourse;
        }
        public async Task<Result<EnrolledCourse>> ArchiveOrUnarchiveEnrolledCourse(Guid userId, Guid courseId, bool setArchived)
        {
            var enrolledCourse = await repo.GetEnrolledCourseByUserIdAndCourseId(userId, courseId);
            if (enrolledCourse is null)
            {
                return Result.Failure<EnrolledCourse>(
                    Error.Create("EnrolledCourse.InvalidUpdate", $"User {userId} is not enrolled course {courseId}")
                );
            }
            enrolledCourse.IsArchived = setArchived;
            await repo.SaveChangeAsync();
            return enrolledCourse;
        }

        public async Task<Result<LearningNote>> AddLearningNote(Guid userId, Guid courseId, Guid lectureId, string content, TimeSpan noteAt)
        {
            var enrolledCourse = await repo.GetEnrolledCourseByUserIdAndCourseId(userId, courseId);
            if (enrolledCourse is null)
            {
                return Result.Failure<LearningNote>(
                    Error.Create("EnrolledCourse.Null", $"User {userId} is not enrolled course {courseId}")
                );
            }
            var note = await repo.GetLearningNoteByTimeSpan(userId, courseId, lectureId, noteAt);
            if(note is null)
            {
                note = new LearningNote
                {
                    UserId = userId,
                    CourseId = courseId,
                    Content = content,
                    LectureId = lectureId,
                    NoteAt = noteAt
                };
                return await repo.AddLearningNote(note);
            }else
            {
                note.Content = content;
                await repo.SaveChangeAsync();
                return note;
            }
        }

        public async Task<Result<LearningNote>> UpdateLearningNote(Guid userId, Guid courseId, Guid lectureId, TimeSpan timeSpan, string content)
        {
            var note = await repo.GetLearningNoteByTimeSpan(userId, courseId, lectureId, timeSpan);
            if(note is null)
            {
                return Result.Failure<LearningNote>(Error.Create("LearningNote.InvalidUpdate", "LearningNote not found"));
            }else
            {
                note.Content = content;
                await repo.SaveChangeAsync();
                return note;
            }
        }

        public async Task<Result> DeleteLearningNote(Guid userId, Guid courseId, Guid lectureId, TimeSpan timeSpan)
        {
            var note = await repo.GetLearningNoteByTimeSpan(userId, courseId, lectureId, timeSpan);
            if (note is null)
            {
                return Result.Failure(Error.Create("LearningNote.Null", "LearningNote not found"));
            }
            else
            {
                var result = await repo.DeleteLearningNote(note);
                return (result > 0) ? Result.Success() : Result.Failure(Error.Create("LearningNote.InvalidDelete", "Something was wrong"));
            }
        }

        public async Task<List<LearningNote>> GetUserLearningNotesForEnrolledCourse(Guid userId, Guid courseId)
        {
            return await repo.GetUserLearningNotesForEnrolledCourse(userId, courseId);
        }

        public async Task<Result<EnrolledCourse>> AddCourseReview(Guid userId, Guid courseId, int rating, string? reviewText)
        {
            var result = await GetEnrolledCourseByUserIdAndCourseId(userId, courseId);
            if (result.IsFailure) return result;
            var courseReview = result.Value.CourseReview;
            if(courseReview is null)
            {
                courseReview = new CourseReview
                {
                    UserId = userId,
                    CourseId = courseId,
                    Rating = rating,
                    ReviewText = reviewText
                };
                result.Value.CourseReview = courseReview;
                await repo.AddCourseReview(courseReview);
                return result.Value;
            }else
            {
                courseReview.Rating = rating;
                courseReview.ReviewText = reviewText;
                await repo.SaveChangeAsync();
                return result.Value;
            }
        }

        public async Task<Result<EnrolledCourse>> UpdateCourseReview(Guid userId, Guid courseId, int rating, string? reviewText)
        {
            var result = await GetEnrolledCourseByUserIdAndCourseId(userId, courseId);
            if (result.IsFailure) return result;
            var courseReview = result.Value.CourseReview;
            courseReview.Rating = rating;
            courseReview.ReviewText = reviewText;
            await repo.SaveChangeAsync();
            return result.Value;
        }

        public async Task<Result> DeleteCourseReview(Guid userId, Guid courseId)
        {
            var result = await GetEnrolledCourseByUserIdAndCourseId(userId, courseId);
            if (result.IsFailure) return result;
            if (result.Value.CourseReview is null) return Result.Success();
            return (await repo.DeleteCourseReview(result.Value.CourseReview)) > 0
                ? Result.Success() 
                : Result.Failure(Error.Create("CourseReview.Error", "Can not delete review"));
        }
    }
}
