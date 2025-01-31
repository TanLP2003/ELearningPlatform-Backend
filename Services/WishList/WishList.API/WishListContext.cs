using Microsoft.EntityFrameworkCore;
using WishList.API.Models;

namespace WishList.API;

public class WishListContext : DbContext
{
    public WishListContext(DbContextOptions<WishListContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<LoveList>()
            .HasKey(ll => ll.UserId);
        modelBuilder.Entity<LoveList>()
            .OwnsMany(ll => ll.Items, builder => { builder.ToJson(); });
    }
    public DbSet<LoveList> LoveLists { get; set; }
}