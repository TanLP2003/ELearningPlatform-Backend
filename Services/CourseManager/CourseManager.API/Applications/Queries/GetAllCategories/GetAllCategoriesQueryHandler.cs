using Application.Messaging;
using CourseManager.Domain.Contracts;
using CourseManager.Domain.Entities;
using Domain;

namespace CourseManager.API.Applications.Queries.GetAllCategories
{
    public class GetAllCategoriesQueryHandler(ICourseRepository repo) : IQueryHandler<GetAllCategoriesQuery, Result<List<Category>>>
    {

        public async Task<Result<List<Category>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await repo.GetAllCategory();
            return categories;
        }
    }
}
