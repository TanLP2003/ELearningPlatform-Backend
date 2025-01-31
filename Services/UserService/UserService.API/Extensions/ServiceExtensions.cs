using EventBus;
using Microsoft.EntityFrameworkCore;
using Minio;
using System.Reflection;
using UserService.API.Infrastructure;
using UserService.API.Infrastructure.Repositories;

namespace UserService.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServiceDependencies(this IServiceCollection services, IConfiguration configuration)
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

            services.AddScoped<IProfileRepository, ProfileRepository>();

            services.AddMinio(configureClient => configureClient
                .WithEndpoint(configuration.GetSection("Minio")["Endpoint"])
                .WithCredentials(configuration.GetSection("Minio")["accessKey"], configuration.GetSection("Minio")["secretKey"])
                .Build());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());
        }
    }
}
