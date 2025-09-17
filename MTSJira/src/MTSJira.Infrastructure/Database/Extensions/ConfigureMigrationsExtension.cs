using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MTSJira.Infrastructure.Database.Contexts;

namespace MTSJira.Infrastructure.Database.Extensions
{
    public static class ConfigureMigrationsExtension
    {
        public static void MigrateDatabase(this IApplicationBuilder app)
        {
            using (IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<JiraDbContext>();
                ILoggerFactory loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                ILogger<ILogger> logger = loggerFactory.CreateLogger<ILogger>();
                var migrationList = context.Database.GetPendingMigrations().ToList();
                if (migrationList.Any())
                {
                    try
                    {
                        logger.LogInformation("Migration beginning");
                        context.Database.Migrate();
                        logger.LogInformation("End of migration");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error when trying to roll out migration");
                        throw;
                    }
                }
                else
                {
                    logger.LogInformation($"No migration was found for {nameof(JiraDbContext)}");
                }
            }
        }
    }
}
