using CourseManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseManager.Infrastructure.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(255);
        builder.HasIndex(c => c.Name)
            .IsUnique();
        builder.HasMany(c => c.SubCategories)
            .WithOne()
            .HasForeignKey(sc => sc.ParentCategoryId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}