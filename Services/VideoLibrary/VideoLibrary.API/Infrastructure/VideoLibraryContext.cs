using Microsoft.EntityFrameworkCore;
using VideoLibrary.API.Models;

namespace VideoLibrary.API.Infrastructure;

public class VideoLibraryContext : DbContext
{
    public VideoLibraryContext(DbContextOptions<VideoLibraryContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FileList>()
            .HasKey(fl => fl.UserId);
        modelBuilder.Entity<FileList>()
            .OwnsMany(fl => fl.Items, builder => builder.ToJson());
    }
    public DbSet<FileList> FileLists { get; set; }
}