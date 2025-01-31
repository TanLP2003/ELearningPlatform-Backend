using Application.Messaging;
using CourseManager.Domain.Entities;
using Domain;

namespace CourseManager.API.Applications.Queries.GetAllCategories;

public sealed record GetAllCategoriesQuery : IQuery<Result<List<Category>>>;