namespace VideoLibrary.API.Models;

public class FileItem
{
    public Guid FileId { get; set; }
    public string FileName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Type { get; set; }
}