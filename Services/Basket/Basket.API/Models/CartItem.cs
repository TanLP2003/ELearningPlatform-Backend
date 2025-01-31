namespace Basket.API.Models;

public class CartItem
{
    public Guid CourseId { get; set; }
    public string? CourseName { get; set; }
    public string? CourseImage { get; set; }
    public Guid AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public decimal Price { get; set; }
}