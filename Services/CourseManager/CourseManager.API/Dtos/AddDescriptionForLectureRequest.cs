using System.ComponentModel.DataAnnotations;

namespace CourseManager.API.Dtos;

public class AddDescriptionForLectureRequest
{
    [Required]
    public Guid CourseId { get; set; }
    [Required]
    public Guid LectureId { get; set; }
    [Required]
    public string Description { get; set; } = default!;
}