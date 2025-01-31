using EventBus;
using Microsoft.EntityFrameworkCore;
using VideoLibrary.API.Application.Services;
using VideoLibrary.API.Infrastructure;
using VideoLibrary.API.Infrastructure.Repositories;

namespace VideoLibrary.API.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureServiceDependency(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                 builder.AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader());
        });
        services.AddDbContext<VideoLibraryContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Postgres"));
        });
        services.AddMessageBroker(configuration, typeof(Program).Assembly);
        services.AddScoped<IVideoLibraryRepository, VideoLibraryRepository>();
        services.AddScoped<IVideoLibraryService, VideoLibraryService>();
    }
}