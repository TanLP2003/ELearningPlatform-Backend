using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions;

public static class MigrationHelper
{
    public static void MigrateDatabase<TDbContext>(this IHost app, int retry = 0)
        where TDbContext : DbContext
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<TDbContext>();
        var logger = services.GetRequiredService<ILogger<TDbContext>>();
        var pendingMigrations = dbContext.Database.GetPendingMigrations();
        if (pendingMigrations.Any())
        {
            try
            {
                logger.LogInformation("Trying to migrate Database ({DbContext})", typeof(TDbContext).Name);
                dbContext.Database.Migrate();
                logger.LogInformation("Database migration ({DbContext}) succeeds", typeof(TDbContext).Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database migration ({DbContext}) failed", typeof(TDbContext).Name);
                if (retry <= 10)
                {
                    Thread.Sleep(2000);
                    app.MigrateDatabase<TDbContext>(retry + 1);
                }
            }
        }else
        {
            logger.LogInformation("No pending migrations");
        }
    }
}