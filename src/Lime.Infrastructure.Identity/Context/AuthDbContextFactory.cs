using Lime.Persistence.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Lime.Infrastructure.Identity.Data.Context;

public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args)
    {
        // Build config
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Database.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        // Get connection string
        var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
        var connectionString = config.GetConnectionString(DatabaseOptions.ConnectionStringName);
        optionsBuilder.UseNpgsql(connectionString);
        return new AuthDbContext(optionsBuilder.Options);
    }
}