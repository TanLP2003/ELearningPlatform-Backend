using CourseManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseManager.Infrastructure.Configurations;
public class LectureConfiguration : IEntityTypeConfiguration<Lecture>
{
    public void Configure(EntityTypeBuilder<Lecture> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Name).IsRequired().HasMaxLength(255);
        builder.Property(l => l.LectureContentUrl).IsRequired(false);
        builder.Property(l => l.VideoName).IsRequired(false);
        builder.Property(l => l.Description).IsRequired(false).HasMaxLength(300);
        builder.Property(l => l.LectureNumber).IsRequired();

        builder.HasMany(l => l.Resources)
            .WithOne()
            .HasForeignKey(r => r.LectureId)
            .IsRequired();
    }
}