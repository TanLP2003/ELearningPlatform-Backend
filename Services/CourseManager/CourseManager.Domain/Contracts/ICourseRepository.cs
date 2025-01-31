using CourseManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Domain.Contracts;

public interface ICourseRepository
{
    Task<List<Course>> GetAllAsync();
    Task CreateCourse(Course course);
    Task CreateSection(Section section);
    Task AddLectureToSection(Lecture lecture);
    Task<Course?> GetById(Guid id);
    Task<Lecture?> GetLectureById(Guid lectureId);

    Task<int> SaveChangeAsync();
    Task<List<Course>> GetMyTeachingCourse(Guid userId);
    Task<List<Category>> GetAllCategory();
    Task<int> UpdateCourseReview(Guid courseId, int reviewCount, double rating);
}