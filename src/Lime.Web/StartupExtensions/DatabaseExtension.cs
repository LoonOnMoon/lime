using Lime.Infrastructure.Data.Context;
using Lime.Infrastructure.Identity.Data;
using Lime.Web.Configuration;

using Microsoft.EntityFrameworkCore;

namespace Lime.Web.StartupExtensions;

public static class DatabaseExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        DatabaseOptions databaseOptions = services.BuildServiceProvider().GetRequiredService<DatabaseOptions>();

        services.AddDbContext<AuthDbContext>(options =>
        {
            options.UseNpgsql(databaseOptions.ConnectionString);

            if (!env.IsProduction())
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddDbContext<LimeDbContext>(options =>
        {
            options.UseNpgsql(databaseOptions.ConnectionString);

            if (!env.IsProduction())
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        });
        return services;
    }
}