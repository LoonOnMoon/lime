using Lime.Domain.Exceptions.Program;
using Lime.Infrastructure.Persistence.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Npgsql;

namespace Lime.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IHostEnvironment env)
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

            // .AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IHostEnvironment env)
    {
        services.AddOptions<DatabaseOptions>()
            .Configure<IConfiguration>(
                (options, configuration) =>
                    {
                        options.ConnectionString = configuration.GetConnectionString(DatabaseOptions.ConnectionStringName)!;

                        try
                        {
                            using (var conn = new NpgsqlConnection(options.ConnectionString))
                            {
                                conn.Open();
                                conn.Close();
                            }
                        }
                        catch (NpgsqlException ex)
                        {
                            throw new StartupException(ex.Message);
                        }
                        catch (Exception ex)
                        {
                            throw new StartupException($"Invalid connection string: {ex.Message}");
                        }
                    })
            .ValidateDataAnnotations();

        services.AddSingleton(sp =>
            {
                try
                {
                    return sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;
                }
                catch (OptionsValidationException ex)
                {
                    throw new StartupException(ex);
                }
            });

        return services;
    }
}