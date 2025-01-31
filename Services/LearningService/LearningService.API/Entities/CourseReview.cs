namespace LearningService.API.Entities
{
    public class CourseReview
    {
        public Guid UserId { get; set;}
        public Guid CourseId { get; set;}
        public int Rating { get; set;}
        public string ReviewText { get; set; }
        public DateOnly CreatedAt { get; set; }

        public EnrolledCourse EnrolledCourse { get; set; }
    }
}
