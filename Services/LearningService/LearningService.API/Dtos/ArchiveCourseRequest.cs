using System.ComponentModel.DataAnnotations;

namespace LearningService.API.Dtos
{
    public class ArchiveCourseRequest
    {
        [Required]
        public Guid CourseId { get; set; }
        [Required]
        public bool SetArchived { get; set; }
    }
}
