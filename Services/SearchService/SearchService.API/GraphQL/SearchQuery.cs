using Microsoft.EntityFrameworkCore;
using SearchService.API.Infrastructure;

namespace SearchService.API.GraphQL
{
    [QueryType]
    public static class SearchQuery
    {
        public static async Task<SearchResult> Search(
            string searchTerm,
            [Service] ApplicationContext dbContext)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new SearchResult { Courses = [], Instructors = [] };

            searchTerm = searchTerm.Trim().ToLower();

            var courses = await dbContext.SearchCourses
                .Where(c =>
                    c.CourseTitle.ToLower().Contains(searchTerm) ||
                    c.InstructorName.ToLower().Contains(searchTerm))
                .OrderBy(c => c.SearchCount)
                .Take(5)
                .ToListAsync();

            var instructors = await dbContext.SearchInstructors
                .Where(i =>
                    i.InstructorName.ToLower().Contains(searchTerm))
                .OrderBy(i => i.SearchCount)
                .Take(5)
                .ToListAsync();

            return new SearchResult
            {
                Courses = courses,
                Instructors = instructors
            };
        }
    }
}
