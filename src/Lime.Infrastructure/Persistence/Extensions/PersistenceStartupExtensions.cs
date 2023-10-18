using Lime.Application.Common.Interfaces.Persistence;
using Lime.Infrastructure.Persistence.Configuration;
using Lime.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lime.Infrastructure.Persistence.Extensions;

public static class PersistenceStartupExtensions
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
            })
            .AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}