using Lime.Persistence.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Lime.Persistence.Context;

public class LimeDbContextFactory : IDesignTimeDbContextFactory<LimeDbContext>
{
    public LimeDbContext CreateDbContext(string[] args)
    {
        // Build config
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Database.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // Get connection string
        var optionsBuilder = new DbContextOptionsBuilder<LimeDbContext>();
        var connectionString = config.GetConnectionString(DatabaseOptions.ConnectionStringName);
        optionsBuilder.UseNpgsql(connectionString);

        return new LimeDbContext(optionsBuilder.Options);
    }
}