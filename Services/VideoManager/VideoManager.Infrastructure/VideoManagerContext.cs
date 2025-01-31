using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoManager.Domain.Entities;
using VideoManager.Domain.Enum;

namespace VideoManager.Infrastructure;

public class VideoManagerContext : DbContext
{
    public VideoManagerContext(DbContextOptions<VideoManagerContext> options) : base(options)
    { 
    }
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    base.OnConfiguring(optionsBuilder);
    //    optionsBuilder.UseNpgsql("Host=localhost;Database=VideoManager;Username=postgres;Password=postgres");
    //}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Video>()
            .HasKey(v => v.Id);
        modelBuilder.Entity<Video>()
            .HasIndex(v => v.UserId);
        modelBuilder.Entity<Video>()
            .Property(v => v.Status).IsRequired()
            .HasConversion(
                s => s.ToString(),
                sDb => (VideoProcessStatus)Enum.Parse(typeof(VideoProcessStatus), sDb)
            );
    }
    public DbSet<Video> Videos { get; set; }
}