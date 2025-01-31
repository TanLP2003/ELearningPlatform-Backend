using System.ComponentModel.DataAnnotations;

namespace Basket.API.DTOs;

public class CartItemDto
{
    [Required]
    public Guid CourseId { get; set; }
    [Required]
    public string? CourseName { get; set; }
    public string? CourseImage { get; set; }
    [Required]
    public Guid AuthorId { get; set; }
    [Required]
    public string? AuthorName { get; set; }
    [Required]
    public decimal Price { get; set; }
}