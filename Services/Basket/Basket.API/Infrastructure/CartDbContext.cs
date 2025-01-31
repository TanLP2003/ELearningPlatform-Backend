using Basket.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Basket.API.Infrastructure;

public class CartDbContext : DbContext
{
    public CartDbContext(DbContextOptions<CartDbContext> options)  
        : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Cart>()
            .HasKey(c => c.UserId);
        modelBuilder.Entity<Cart>()
            .OwnsMany(c => c.Items, builder => { builder.ToJson(); });
        modelBuilder.Entity<OutboxMessage>()
            .HasKey(o => o.EventId);
        //modelBuilder.Entity<OutboxMessage>()
        //    .Property(o => o.EventType)
        //    .IsRequired();
        //modelBuilder.Entity<OutboxMessage>()
        //    .Property(o => o.Content).IsRequired();
        //modelBuilder.Entity<OutboxMessage>()
        //    .Property(o => o.OccurOn).IsRequired()
    }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
}