using EventBus;
using Microsoft.EntityFrameworkCore;
using SearchService.API.Infrastructure;
using SearchService.API.Infrastructure.Repositories;
using System.Reflection;

namespace SearchService.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServiceDependency(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                     builder.AllowAnyOrigin()
                             .AllowAnyMethod()
                             .AllowAnyHeader());
            });
            service.AddDbContext<ApplicationContext>(option =>
            {
                option.UseNpgsql(configuration.GetConnectionString("Postgres"));
            });
            service.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());
            service.AddScoped<ISearchRepo, SearchRepo>();
        }
    }
}
