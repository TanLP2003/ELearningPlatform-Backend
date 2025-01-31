using CourseManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseManager.Infrastructure.Configurations
{
    public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder.HasKey(sc => sc.Id);                
            builder.Property(sc => sc.Name).IsRequired();
            builder.HasIndex(sc => sc.Name).IsUnique();
            builder.HasIndex(sc => sc.ParentCategoryId);
        }
    }
}
