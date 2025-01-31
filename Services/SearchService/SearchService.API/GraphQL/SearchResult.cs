using SearchService.API.Entities;

namespace SearchService.API.GraphQL
{
    public class SearchResult
    {
        public IEnumerable<SearchCourse> Courses { get; set; }
        public IEnumerable<SearchInstructor> Instructors { get; set; }
    }
}
