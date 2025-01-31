using Microsoft.EntityFrameworkCore;
using SearchService.API.Entities;

namespace SearchService.API.Infrastructure
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

        public DbSet<SearchCourse> SearchCourses { get; set; }
        public DbSet<SearchInstructor> SearchInstructors { get; set; }
    }
}
