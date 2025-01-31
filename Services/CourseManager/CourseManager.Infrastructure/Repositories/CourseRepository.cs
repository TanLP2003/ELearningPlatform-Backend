using CourseManager.Domain.Contracts;
using CourseManager.Domain.Entities;
using CourseManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Infrastructure.Repositories;

public class CourseRepository(CourseDbContext context) : ICourseRepository
{
    public async Task CreateCourse(Course course)
    {
        await context.Courses.AddAsync(course);
    }
    public async Task<Course?> GetById(Guid id)
    {
        var course = await context.Courses.Where(c => c.Id == id)
            .Include(c => c.Metadata)
            .Include(c => c.Sections)
            .ThenInclude(s => s.Lectures)
            .FirstOrDefaultAsync();
        return course;
    }
    public async Task CreateSection(Section section)
    {
        await context.Sections.AddAsync(section);   
    }
    public async Task AddLectureToSection(Lecture lecture)
    {
        await context.Lectures.AddAsync(lecture);
    }
    public async Task<Lecture?> GetLectureById(Guid lectureId)
    {
        return await context.Lectures.FirstOrDefaultAsync(l => l.Id == lectureId);
    }

    public async Task<List<Category>> GetAllCategories()
    {
        return await context.Categories.ToListAsync();
    }

    public async Task<int> SaveChangeAsync()
    {
        return await context.SaveChangesAsync();
    }

    public async Task<List<Course>> GetMyTeachingCourse(Guid userId)
    {
        var courses = await context.Courses.Where(c => c.InstructorId == userId)
            .Include(c => c.Metadata)
            .Include(c => c.Sections)
            .ThenInclude(s => s.Lectures)
            .ToListAsync();
        return courses;
    }

    public async Task<List<Course>> GetAllAsync()
    {
        var courses = await context.Courses.Where(c => c.Visuability == CourseVisuability.Public)
            .Include(c => c.Metadata)
            .Include(c => c.Sections)
            .ThenInclude(s => s.Lectures)
            .ToListAsync();
        return courses;
    }

    public async Task<List<Category>> GetAllCategory()
    {
        var categories = await context.Categories.Include(category => category.SubCategories).ToListAsync();
        return categories;
    }

    public async Task<int> UpdateCourseReview(Guid courseId, int reviewCount, double rating)
    {
        var courseMetadata = await context.CoursesMetadata.FirstOrDefaultAsync(cm => cm.CourseId == courseId);
        if(courseMetadata is null)
        {
            courseMetadata = CourseMetadata.Create(courseId, rating, reviewCount, 0);
            await context.CoursesMetadata.AddAsync(courseMetadata);
            return await context.SaveChangesAsync();
        }else
        {
            courseMetadata.ReviewCount = reviewCount;
            courseMetadata.Rating = rating;
            return await context.SaveChangesAsync();
        }
    }
}