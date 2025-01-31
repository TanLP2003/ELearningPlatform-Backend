using CourseManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseManager.Infrastructure.Configurations
{
    public class CourseMetadataConfiguration : IEntityTypeConfiguration<CourseMetadata>
    {
        public void Configure(EntityTypeBuilder<CourseMetadata> builder)
        {
            builder.HasKey(cm => cm.Id);
            builder.Property(cm => cm.ReviewCount)
                .IsRequired()
                .HasDefaultValue(0);
            builder.Property(cm => cm.Rating)
                .IsRequired()
                .HasDefaultValue(0);
            builder.Property(cm => cm.TotalStudent)
                .IsRequired()
                .HasDefaultValue(0);
        }
    }
}
