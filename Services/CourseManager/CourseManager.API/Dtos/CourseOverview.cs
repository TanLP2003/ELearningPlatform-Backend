using CourseManager.Domain.Enums;

namespace CourseManager.API.Dtos;

public class CourseOverview
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public string CategoryId { get; set; }
    public string Level { get; set; }
    public string? Language { get; set; }
    public string? CourseImage { get; set; }
    public int Price { get; set; }
    public List<SectionOverview> Sections { get; set; }
    public Guid InstructorId { get; set; }
    public string InstructorName { get; set; }
    public CourseMetadataDto Metadata { get; set; }
}

public class SectionOverview
{
    public string Name { get; set; }
    public List<LectureOverview> Lectures { get; set; }
}

public class LectureOverview
{
    public string Name { get; set; }
    public string? Description { get; set; }
}
