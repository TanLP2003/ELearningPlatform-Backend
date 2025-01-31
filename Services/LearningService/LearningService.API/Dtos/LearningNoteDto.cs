using System.ComponentModel.DataAnnotations;

namespace LearningService.API.Dtos
{
    public class LearningNoteDto
    {
        [Required]
        public Guid CourseId { get; set; }
        [Required]
        public Guid LectureId { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]  
        public TimeSpan NoteAt { get; set; }
    }
}
