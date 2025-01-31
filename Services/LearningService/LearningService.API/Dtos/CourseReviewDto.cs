using System.ComponentModel.DataAnnotations;

namespace LearningService.API.Dtos
{
    public class CourseReviewDto
    {
        [Required]
        public Guid CourseId { get; set; }
        [Required]
        public int Rating { get; set; }
        public string ReviewText { get; set;}
    }
}
