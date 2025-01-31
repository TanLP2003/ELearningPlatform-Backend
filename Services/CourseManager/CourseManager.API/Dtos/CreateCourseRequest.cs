using System.ComponentModel.DataAnnotations;

namespace CourseManager.API.Dtos;

public class CreateCourseRequest
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Level { get; set; }
    [Required]
    public string InstructorName { get; set;}
    [Required]
    public Guid CategoryId { get; set; }
}