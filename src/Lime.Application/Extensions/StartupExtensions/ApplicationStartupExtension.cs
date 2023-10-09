using System.Reflection;

using FluentValidation;

using Lime.Application.Common.Behaviors;

using Mapster;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace Lime.Application.Extensions.StartupExtensions;

public static class ApplicationStartupExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services, TypeAdapterConfig typeAdapterConfig)
    {
        services.AddMappings(typeAdapterConfig)
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationStartupExtension).Assembly));

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}