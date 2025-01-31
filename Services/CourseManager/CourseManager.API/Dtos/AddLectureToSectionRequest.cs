using System.ComponentModel.DataAnnotations;

namespace CourseManager.API.Dtos;

public class AddLectureToSectionRequest
{
    [Required]
    public Guid CourseId { get; set; }
    [Required]
    public Guid SectionId { get; set; }
    [Required]
    public string? Title { get; set; }
}