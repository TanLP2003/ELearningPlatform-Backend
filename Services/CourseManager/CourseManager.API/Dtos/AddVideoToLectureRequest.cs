namespace CourseManager.API.Dtos;

public class AddVideoToLectureRequest
{
    public Guid VideoId { get; set; }
    public Guid LectureId { get; set; }
    public Guid CourseId { get; set; }
}