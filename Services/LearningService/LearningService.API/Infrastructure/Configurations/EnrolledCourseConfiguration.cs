using LearningService.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningService.API.Infrastructure.Configurations
{
    public class EnrolledCourseConfiguration : IEntityTypeConfiguration<EnrolledCourse>
    {
        public void Configure(EntityTypeBuilder<EnrolledCourse> builder)
        {
            builder.HasKey(ec => new { ec.UserId, ec.CourseId });
            builder.HasOne(ec => ec.CourseReview)
                .WithOne(cr => cr.EnrolledCourse)
                .HasForeignKey<CourseReview>(cr => new { cr.UserId, cr.CourseId });

            builder.Property(e => e.IsArchived)
                .HasDefaultValue(false);
        }
    }
}
