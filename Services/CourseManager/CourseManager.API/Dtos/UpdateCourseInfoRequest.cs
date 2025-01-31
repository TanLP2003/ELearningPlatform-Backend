namespace CourseManager.API.Dtos;

public class UpdateCourseInfoRequest
{
    public string? Description { get; set; } = default!;
    public string? Title { get; set; } = default!;
    public string? Level { get; set; } = default!;
    public int? Price { get; set; } = default!;
    public Guid? Category { get; set; } = default!;
    public string? Language { get; set; } = default!;
}