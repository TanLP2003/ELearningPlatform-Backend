using CourseManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseManager.Infrastructure.Configurations;
public class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(150);
        builder.Property(s => s.SectionNumber).IsRequired();
        builder.HasMany(s => s.Lectures)
            .WithOne()
            .HasForeignKey(l => l.SectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}