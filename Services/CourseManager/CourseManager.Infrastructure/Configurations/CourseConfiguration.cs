using CourseManager.Domain.Entities;
using CourseManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseManager.Infrastructure.Configuration;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Title).HasMaxLength(150).IsRequired();
        builder.Property(c => c.CategoryId).IsRequired();
        builder.Property(c => c.Level).IsRequired()
            .HasConversion(
                l => l.ToString(),
                v => (CourseLevel)Enum.Parse(typeof(CourseLevel), v)
            );
        builder.Property(c => c.Visuability).IsRequired()
            .HasConversion(
                vi => vi.ToString(),
                vidb => (CourseVisuability)Enum.Parse(typeof(CourseVisuability), vidb)
            );
        builder.HasMany(c => c.Sections)
            .WithOne()
            .HasForeignKey(s => s.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(c => c.CourseImage).IsRequired(false);
        builder.Property(c => c.InstructorId).IsRequired();
        builder.Property(c => c.InstructorName).IsRequired();
        builder.HasOne(c => c.Category)
            .WithMany()
            .HasForeignKey(c => c.CategoryId)
            .IsRequired();

        builder.HasOne(c => c.SubCategory)
            .WithMany()
            .HasForeignKey(c => c.SubCategoryId)
            .IsRequired(false);

        builder.HasOne(c => c.Metadata)
            .WithOne()
            .HasForeignKey<CourseMetadata>(cm => cm.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}