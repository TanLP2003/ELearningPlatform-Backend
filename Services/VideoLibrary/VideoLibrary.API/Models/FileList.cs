namespace VideoLibrary.API.Models;

public class FileList
{
    public Guid UserId { get; set; }
    public List<FileItem> Items { get; set; } = [];
}