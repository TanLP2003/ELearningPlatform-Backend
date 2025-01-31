using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Domain.Entities
{
    public class CourseMetadata : Entity<Guid>
    {
        public Guid CourseId { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public int TotalStudent { get; set; }
        private CourseMetadata(Guid id, Guid courseId, double rating, int reviewCount, int totalStudent) : base(id)
        {
            CourseId = courseId;
            Rating = rating;
            ReviewCount = reviewCount;
        }

        public static CourseMetadata Create(Guid courseId, double rating, int reviewCount, int totalStudent)
        {
            var newCourseMetadata = new CourseMetadata(Guid.NewGuid(), courseId, rating, reviewCount, totalStudent);
            return newCourseMetadata;
        }
    }
}
