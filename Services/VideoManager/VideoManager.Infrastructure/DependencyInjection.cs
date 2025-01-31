using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoManager.Domain.Contracts;
using VideoManager.Infrastructure.Repositories;

namespace VideoManager.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureService(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddDbContext<VideoManagerContext>(config =>
        {
            config.UseNpgsql(configuration.GetConnectionString("Postgres"));
        });
        service.AddScoped<ILocalStorageRepository, LocalStorageRepository>();
        service.AddScoped<IVideoRepository, VideoRepository>();
    }
}