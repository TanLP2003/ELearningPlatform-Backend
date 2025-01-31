using System.ComponentModel.DataAnnotations;

namespace SearchService.API.Entities
{
    public class SearchInstructor
    {
        [Key]
        public Guid InstructorId { get; set; }
        [Required]
        public string InstructorName { get; set; }
        public string? Avatar { get; set; }
        public int SearchCount { get; set; }
    }
}
