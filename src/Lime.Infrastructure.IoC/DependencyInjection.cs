using Lime.Infrastructure.Identity.Extensions.ConfigurationExtensions;
using Lime.Infrastructure.Identity.Extensions.StartupExtensions;
using Lime.Persistence.Extensions.ConfigurationExtensions;
using Lime.Persistence.Extensions.StartupExtensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lime.Infrastructure.IoC;

public static class DependencyInjection
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IHostEnvironment env)
    {
        services.AddDatabaseConfiguration(env);
        services.AddDatabase(env);

        services.AddJwtConfiguration();
        services.AddIdentity(env);

        return services;

        // services.AddHttpContextAccessor();
    }

    public static IApplicationBuilder RegisterMiddleware(this IApplicationBuilder app)
    {
        app.UseIdentity();

        return app;
    }
}