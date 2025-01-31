using System.ComponentModel.DataAnnotations;

namespace SearchService.API.Entities
{
    public class SearchCourse
    {
        [Key]
        public Guid CourseId { get; set; }
        [Required]
        public string CourseTitle { get; set; }
        public string? CourseImage { get; set; }
        [Required]
        public string InstructorName { get; set; }
        public int SearchCount { get; set; }
    }
}
