using LearningService.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningService.API.Infrastructure.Configurations
{
    public class CourseReviewConfiguration : IEntityTypeConfiguration<CourseReview>
    {
        public void Configure(EntityTypeBuilder<CourseReview> builder)
        {
            builder.HasKey(cr => new {cr.UserId, cr.CourseId});
            builder.Property(cr => cr.Rating)
                .IsRequired()
                .HasDefaultValue(0);
            
        }
    }
}
