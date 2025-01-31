using EventBus;
using LearningService.API.Applications.Services;
using LearningService.API.Infrastructure;
using LearningService.API.Infrastructure.Repositories;
using LearningService.API.Protos;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LearningService.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServiceDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                     builder.AllowAnyOrigin()
                             .AllowAnyMethod()
                             .AllowAnyHeader());
            });
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Postgres"));
            });
            services.AddScoped<ILearningRepo, LearningRepo>();
            services.AddScoped<ILearningServices, LearningServices>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());
            services.AddGrpc();
            services.AddGrpcClient<CourseManagerProtoService.CourseManagerProtoServiceClient>(options =>
            {
                options.Address = new Uri(configuration["GrpcSettings:CourseUri"]);
            });
        }
    }
}
