using Microsoft.EntityFrameworkCore;
using UserService.API.Models;

namespace UserService.API.Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Profile>()
                .HasKey(p => p.Id);

            builder.Entity<Profile>()
                .Property(p => p.FirstName)
                .IsRequired();

            builder.Entity<Profile>()
                .Property(p => p.LastName)
                .IsRequired();

        }

        public DbSet<Profile> Profiles { get; set; }
    }
}
