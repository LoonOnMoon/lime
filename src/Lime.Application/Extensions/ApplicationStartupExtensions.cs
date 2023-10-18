using System.Reflection;

using FluentValidation;

using Lime.Application.Common.Behaviors;

using Mapster;

using MapsterMapper;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace Lime.Application.Extensions;

public static class ApplicationStartupExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, TypeAdapterConfig typeAdapterConfig)
    {
        typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationStartupExtensions).Assembly));

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}