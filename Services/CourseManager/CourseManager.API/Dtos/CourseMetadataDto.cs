namespace CourseManager.API.Dtos
{
    public class CourseMetadataDto
    {
        public Guid CourseId { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public int TotalStudent { get; set; }
    }
}
