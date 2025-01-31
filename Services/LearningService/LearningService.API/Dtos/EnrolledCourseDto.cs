namespace LearningService.API.Dtos
{
    public class EnrolledCourseDto
    {
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseImage { get; set; }
        public Guid InstructorId { get; set; }
        public string InstructorName { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public DateTime? LastAccessed { get; set; }
        public float? CompletionPercentage { get; set; }
        public bool IsArchived { get; set; } = false;

        public CourseReviewDto CourseReview { get; set; }
    }
}
