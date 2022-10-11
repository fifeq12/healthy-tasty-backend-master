using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HealthyTasty.Infrastructure.EntityFramework
{
    public static class PostgresExtension
    {
        public const string SectionName = "postgres";

        public static void AddPostgres<T>(this IServiceCollection serviceCollection, PostgresConfig postgresConfig)
            where T : DbContext
        {
            serviceCollection.AddDbContextFactory<T>(config =>
            {
                config.UseNpgsql(@$"
                    Host={postgresConfig.Host};
                    Database={postgresConfig.Database};
                    Username={postgresConfig.Username};
                    Password={postgresConfig.Password}");

                config.UseSnakeCaseNamingConvention();
                config.EnableDetailedErrors();
                config.EnableSensitiveDataLogging();
            });
        }
    }
}
