using CourseManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseManager.Infrastructure;
public class CourseDbContext : DbContext
{
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseNpgsql(configuration.GetConnectionString("Postgres"));
    //}
    public CourseDbContext(DbContextOptions<CourseDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CourseDbContext).Assembly);
        var developmentCategory = Category.Create("Phát triển");
        var businessCategory = Category.Create("Kinh doanh");
        var designCategory = Category.Create("Thiết kế");
        modelBuilder.Entity<Category>()
            .HasData(developmentCategory, businessCategory, designCategory);
        var webDevelopmentSubCategory = SubCategory.Create(developmentCategory.Id, "Phát triển Web");
        var dsSubCategory = SubCategory.Create(developmentCategory.Id, "Khoa học dữ liệu");
        var mbSubCategory = SubCategory.Create(developmentCategory.Id, "Phát triển mobile");
        var communicationSubCategory = SubCategory.Create(businessCategory.Id, "Giao tiếp");
        var webDesignSubCategory = SubCategory.Create(designCategory.Id, "Thiết kế Web");
        modelBuilder.Entity<SubCategory>()
            .HasData(webDevelopmentSubCategory, dsSubCategory,  mbSubCategory, communicationSubCategory, webDesignSubCategory);
    }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Lecture> Lectures { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<CourseMetadata> CoursesMetadata { get; set;}
}