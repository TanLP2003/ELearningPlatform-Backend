using LearningService.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningService.API.Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
        }

        public DbSet<EnrolledCourse> EnrolledCourses { get; set; }
        public DbSet<CourseReview> CourseReviews { get; set; }
        public DbSet<LearningNote> LearningNotes { get; set; }
    }
}
