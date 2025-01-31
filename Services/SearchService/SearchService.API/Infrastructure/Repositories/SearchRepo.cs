using SearchService.API.Entities;

namespace SearchService.API.Infrastructure.Repositories
{
    public class SearchRepo(ApplicationContext dbContext) : ISearchRepo
    {
        public async Task CreateSearchInstructor(SearchInstructor searchInstructor)
        {
            await dbContext.SearchInstructors.AddAsync(searchInstructor);
            await dbContext.SaveChangesAsync();
        }

        public async Task CreateSearchCourse(SearchCourse searchCourse)
        {
            await dbContext.SearchCourses.AddAsync(searchCourse);
            await dbContext.SaveChangesAsync();
        }
    }
}
