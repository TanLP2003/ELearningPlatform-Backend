using LearningService.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningService.API.Infrastructure.Configurations
{
    public class LearningNoteConfiguration : IEntityTypeConfiguration<LearningNote>
    {
        public void Configure(EntityTypeBuilder<LearningNote> builder)
        {
            builder.HasKey(ln => new { ln.UserId, ln.CourseId, ln.LectureId, ln.NoteAt });

            builder.HasOne(ln => ln.EnrolledCourse)
                .WithMany(ec => ec.LearningNotes)
                .HasForeignKey(ln => new { ln.UserId, ln.CourseId });

        }
    }
}
