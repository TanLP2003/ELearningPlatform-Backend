using System.ComponentModel.DataAnnotations;

namespace CourseManager.API.Dtos;

public class AddSectionToCourseRequest
{
    [Required]
    public Guid CourseId { get; set; }
    [Required]
    public string SectionName { get; set; }
}