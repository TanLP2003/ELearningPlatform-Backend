using SearchService.API.Entities;

namespace SearchService.API.Infrastructure.Repositories
{
    public interface ISearchRepo
    {
        Task CreateSearchInstructor(SearchInstructor searchInstructor);
        Task CreateSearchCourse(SearchCourse searchCourse);
    }
}
