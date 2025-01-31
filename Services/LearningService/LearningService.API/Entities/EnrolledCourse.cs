namespace LearningService.API.Entities
{
    public class EnrolledCourse
    {
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public DateTime? LastAccessed { get; set; }
        public float? CompletionPercentage { get; set; }
        public bool IsArchived { get; set; } = false;

        public CourseReview CourseReview { get; set; }
        public ICollection<LearningNote> LearningNotes { get; set; }
    }
}
