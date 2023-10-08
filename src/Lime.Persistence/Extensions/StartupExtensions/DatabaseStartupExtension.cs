using Lime.Persistence.Configuration;
using Lime.Persistence.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lime.Persistence.Extensions.StartupExtensions;

public static class DatabaseStartupExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IHostEnvironment env)
    {
        DatabaseOptions databaseOptions = services.BuildServiceProvider().GetRequiredService<DatabaseOptions>();

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