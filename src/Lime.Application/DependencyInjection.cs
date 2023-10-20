using System.Reflection;

using Lime.Application.Common.Behaviors;

using Microsoft.Extensions.DependencyInjection;

namespace Lime.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, TypeAdapterConfig typeAdapterConfig)
    {
        typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}