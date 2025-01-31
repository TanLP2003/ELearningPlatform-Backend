using System.ComponentModel.DataAnnotations;

namespace LearningService.API.Entities
{
    public class LearningNote
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid CourseId { get; set; }
        [Required]
        public Guid LectureId { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public TimeSpan NoteAt { get; set; }

        public EnrolledCourse EnrolledCourse { get; set; }
    }
}
