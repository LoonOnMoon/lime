using Lime.Web.Configuration;
using Lime.Web.Models.Exceptions;

using Microsoft.Extensions.Options;

using Npgsql;

namespace Lime.Web.StartupExtensions.ConfigurationExtensions;

public static class DatabaseConfigurationExtension
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services)
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
                    throw ConfigurationValidator.GetStartupValidationException(ex);
                }
            });

        return services;
    }
}