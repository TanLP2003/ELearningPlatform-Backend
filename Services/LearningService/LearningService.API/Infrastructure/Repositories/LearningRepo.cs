using LearningService.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningService.API.Infrastructure.Repositories
{
    public class LearningRepo(ApplicationContext dbContext) : ILearningRepo
    {
        public async Task<EnrolledCourse> AddEnrolledCourse(EnrolledCourse enrolledCourse)
        {
            dbContext.EnrolledCourses.Add(enrolledCourse);
            await dbContext.SaveChangesAsync();
            return enrolledCourse;
        }

        public async Task<List<EnrolledCourse>> AddManyEnrolledCourses(List<EnrolledCourse> enrolledCourses)
        {
            await dbContext.EnrolledCourses.AddRangeAsync(enrolledCourses);
            await dbContext.SaveChangesAsync();
            return enrolledCourses;
        }

        public async Task<List<EnrolledCourse>> GetAllEnrolledCoursesByUserId(Guid userId)
        {
            var enrolledCourses = await dbContext.EnrolledCourses
                .Where(ec => ec.UserId == userId)
                .Include(ec => ec.CourseReview)
                .ToListAsync();
            return enrolledCourses;
        }

        public async Task<EnrolledCourse?> GetEnrolledCourseByUserIdAndCourseId(Guid userId, Guid courseId)
        {
            return await dbContext.EnrolledCourses.Include(ec => ec.CourseReview).FirstOrDefaultAsync(ec => ec.UserId == userId && ec.CourseId == courseId);
        }

        public async Task<LearningNote> AddLearningNote(LearningNote learningNote)
        {
            await dbContext.LearningNotes.AddAsync(learningNote);
            await dbContext.SaveChangesAsync();
            return learningNote;
        }

        public async Task<LearningNote?> GetLearningNoteByTimeSpan(Guid userId, Guid courseId, Guid lectureId, TimeSpan duration)
        {
            return await dbContext.LearningNotes
                .FirstOrDefaultAsync(ln => ln.UserId == userId && ln.CourseId == courseId 
                                        && ln.LectureId == lectureId 
                                        && ln.NoteAt == duration);
        }

        public async Task<int> DeleteLearningNote(LearningNote learningNote)
        {
            dbContext.LearningNotes.Remove(learningNote);
            return await dbContext.SaveChangesAsync();
            
        }

        public async Task<List<LearningNote>> GetUserLearningNotesForEnrolledCourse(Guid userId, Guid courseId)
        {
            var listLearningNotes = await dbContext.LearningNotes
                .Where(ln => ln.UserId == userId && ln.CourseId == courseId)
                .ToListAsync();
            return listLearningNotes;
        }

        public async Task<CourseReview> AddCourseReview(CourseReview review)
        {
            await dbContext.CourseReviews.AddAsync(review);
            await dbContext.SaveChangesAsync();
            return review;
        }

        public async Task<CourseReview> UpdateCourseReview(CourseReview review)
        {
            
            await dbContext.SaveChangesAsync();
            return review;
        }

        public async Task<int> DeleteCourseReview(CourseReview courseReview)
        {
            dbContext.CourseReviews.Remove(courseReview);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<(int, double)> GetTotalReviewForCourse(Guid courseId)
        {
            var reviews = await dbContext.CourseReviews
                .Where(cr => cr.CourseId == courseId)
                .ToListAsync();

            int totalReviews = reviews.Count;
            double averageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;

            return (totalReviews, averageRating);
        }

        public async Task<int> SaveChangeAsync() => await dbContext.SaveChangesAsync();
    }
}
