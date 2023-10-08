using Lime.Application.Extensions.StartupExtensions;
using Lime.Infrastructure.Identity.Extensions.ConfigurationExtensions;
using Lime.Infrastructure.Identity.Extensions.StartupExtensions;
using Lime.Persistence.Extensions.ConfigurationExtensions;
using Lime.Persistence.Extensions.StartupExtensions;

using Mapster;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lime.Infrastructure.IoC;

public static class DependencyInjection
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IHostEnvironment env)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;

        services.AddDatabaseConfiguration(env)
            .AddDatabase(env)
            .AddJwtConfiguration()
            .AddIdentity(env, typeAdapterConfig)
            .AddApplication(typeAdapterConfig);

        // services.AddHttpContextAccessor();

        return services;
    }

    public static IApplicationBuilder RegisterMiddleware(this IApplicationBuilder app)
    {
        app.UseIdentity();

        return app;
    }
}