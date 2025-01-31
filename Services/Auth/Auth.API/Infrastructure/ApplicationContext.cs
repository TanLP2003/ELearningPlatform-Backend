using Auth.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AuthUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<AuthUser>()
                .HasOne(u => u.RefreshToken)
                .WithOne()
                .HasForeignKey<AuthRefreshToken>(t => t.UserId);

            builder.Entity<AuthRefreshToken>()
                .HasIndex(rt => rt.UserId);

            builder.Entity<AuthRefreshToken>()
                .Property(t => t.Token)
                .IsRequired();

            builder.Entity<AuthRefreshToken>()
                .Property(t => t.ExpiredAt)
                .IsRequired();
        }

        public DbSet<AuthUser> Users { get; set; }
        public DbSet<AuthRefreshToken> RefreshTokens { get; set; }
    }
}
